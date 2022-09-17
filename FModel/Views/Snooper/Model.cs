using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Animation;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse_Conversion.Meshes.PSK;
using Silk.NET.OpenGL;

namespace FModel.Views.Snooper;

public class Model : IDisposable
{
    private uint _handle;
    private GL _gl;

    private BufferObject<uint> _ebo;
    private BufferObject<float> _vbo;
    private BufferObject<float> _morphVbo;
    private BufferObject<Matrix4x4> _matrixVbo;
    private VertexArrayObject<float, uint> _vao;

    private uint _vertexSize = 9; // VertexIndex + Position + Normal + UV
    private const uint _faceSize = 3; // just so we don't have to do .Length
    private readonly uint[] _facesIndex = { 1, 0, 2 };

    public readonly UObject Owner;
    public readonly string Name;
    public readonly string Type;
    public readonly bool HasVertexColors;
    public readonly bool HasBones;
    public readonly bool HasMorphTargets;
    public uint[] Indices;
    public float[] Vertices;
    public Section[] Sections;
    public readonly Morph[] Morphs;
    public readonly List<CSkelMeshBone> Skeleton;

    public int TransformsCount;
    public readonly List<Transform> Transforms;

    public bool Show;
    public bool IsSelected;
    public bool IsSavable;
    public bool DisplayVertexColors;
    public bool DisplayBones;
    public int SelectedMorph;
    public float MorphTime;

    protected Model(UObject owner, string name, string type)
    {
        Owner = owner;
        Name = name;
        Type = type;
        Transforms = new List<Transform>();
        Show = true;
        IsSavable = owner is not UWorld;
    }

    public Model(UObject owner, string name, string type, CBaseMeshLod lod, CMeshVertex[] vertices, FPackageIndex[] morphTargets = null, List<CSkelMeshBone> skeleton = null, Transform transform = null)
        : this(owner, name, type)
    {
        HasVertexColors = lod.VertexColors != null;
        if (HasVertexColors) _vertexSize += 4; // + Color

        Skeleton = skeleton;
        HasBones = Skeleton != null;
        if (HasBones) _vertexSize += 8; // + BoneIds + BoneWeights

        var sections = lod.Sections.Value;
        Sections = new Section[sections.Length];
        Indices = new uint[sections.Sum(section => section.NumFaces * _faceSize)];
        Vertices = new float[Indices.Length * _vertexSize];

        for (var s = 0; s < sections.Length; s++)
        {
            var section = sections[s];
            Sections[s] = new Section(section.MaterialName, section.MaterialIndex, (uint) section.NumFaces * _faceSize, section.FirstIndex, section);
            for (uint face = 0; face < section.NumFaces; face++)
            {
                foreach (var f in _facesIndex)
                {
                    var count = 0;
                    var i = face * _faceSize + f;
                    var index = section.FirstIndex + i;
                    var baseIndex = index * _vertexSize;
                    var indice = lod.Indices.Value[index];

                    var vert = vertices[indice];
                    Vertices[baseIndex + count++] = indice;
                    Vertices[baseIndex + count++] = vert.Position.X * Constants.SCALE_DOWN_RATIO;
                    Vertices[baseIndex + count++] = vert.Position.Z * Constants.SCALE_DOWN_RATIO;
                    Vertices[baseIndex + count++] = vert.Position.Y * Constants.SCALE_DOWN_RATIO;
                    Vertices[baseIndex + count++] = vert.Normal.X;
                    Vertices[baseIndex + count++] = vert.Normal.Z;
                    Vertices[baseIndex + count++] = vert.Normal.Y;
                    Vertices[baseIndex + count++] = vert.UV.U;
                    Vertices[baseIndex + count++] = vert.UV.V;

                    if (HasVertexColors)
                    {
                        var color = lod.VertexColors[indice];
                        Vertices[baseIndex + count++] = color.R;
                        Vertices[baseIndex + count++] = color.G;
                        Vertices[baseIndex + count++] = color.B;
                        Vertices[baseIndex + count++] = color.A;
                    }

                    if (HasBones)
                    {
                        var skelVert = (CSkelMeshVertex) vert;
                        var weightsHash = skelVert.UnpackWeights();
                        Vertices[baseIndex + count++] = skelVert.Bone[0];
                        Vertices[baseIndex + count++] = skelVert.Bone[1];
                        Vertices[baseIndex + count++] = skelVert.Bone[2];
                        Vertices[baseIndex + count++] = skelVert.Bone[3];
                        Vertices[baseIndex + count++] = weightsHash[0];
                        Vertices[baseIndex + count++] = weightsHash[1];
                        Vertices[baseIndex + count++] = weightsHash[2];
                        Vertices[baseIndex + count++] = weightsHash[3];
                    }

                    Indices[index] = i;
                }
            }
        }

        HasMorphTargets = morphTargets != null;
        if (HasMorphTargets)
        {
            Morphs = new Morph[morphTargets.Length];
            for (var i = 0; i < Morphs.Length; i++)
            {
                Morphs[i] = new Morph(Vertices, _vertexSize, morphTargets[i].Load<UMorphTarget>());
            }
        }

        AddInstance(transform ?? Transform.Identity);
    }

    public void AddInstance(Transform transform) => Transforms.Add(transform);

    public void UpdateMatrix(int index)
    {
        _matrixVbo.Bind();
        _matrixVbo.Update(index, Transforms[index].Matrix);
        _matrixVbo.Unbind();
    }

    public void UpdateMorph()
    {
        _morphVbo.Bind();
        _morphVbo.Update(Morphs[SelectedMorph].Vertices);
        _morphVbo.Unbind();
    }

    public void Setup(GL gl)
    {
        _gl = gl;

        _handle = _gl.CreateProgram();

        _ebo = new BufferObject<uint>(_gl, Indices, BufferTargetARB.ElementArrayBuffer);
        _vbo = new BufferObject<float>(_gl, Vertices, BufferTargetARB.ArrayBuffer);
        _vao = new VertexArrayObject<float, uint>(_gl, _vbo, _ebo);

        _vao.VertexAttributePointer(0, 1, VertexAttribPointerType.Int, _vertexSize, 0); // vertex index
        _vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, _vertexSize, 1); // position
        _vao.VertexAttributePointer(2, 3, VertexAttribPointerType.Float, _vertexSize, 4); // normal
        _vao.VertexAttributePointer(3, 2, VertexAttribPointerType.Float, _vertexSize, 7); // uv
        _vao.VertexAttributePointer(4, 4, VertexAttribPointerType.Float, _vertexSize, 9); // color
        _vao.VertexAttributePointer(5, 4, VertexAttribPointerType.Int, _vertexSize, 13); // boneids
        _vao.VertexAttributePointer(6, 4, VertexAttribPointerType.Float, _vertexSize, 17); // boneweights

        {   // instanced models transform
            TransformsCount = Transforms.Count;
            var instanceMatrix = new Matrix4x4[TransformsCount];
            for (var i = 0; i < instanceMatrix.Length; i++)
                instanceMatrix[i] = Transforms[i].Matrix;
            _matrixVbo = new BufferObject<Matrix4x4>(_gl, instanceMatrix, BufferTargetARB.ArrayBuffer);
            _vao.BindInstancing(); // VertexAttributePointer 7, 8, 9, 10
        }

        if (HasMorphTargets)
        {
            for (uint morph = 0; morph < Morphs.Length; morph++)
            {
                Morphs[morph].Setup(gl);
                if (morph == SelectedMorph)
                    _morphVbo = new BufferObject<float>(_gl, Morphs[morph].Vertices, BufferTargetARB.ArrayBuffer);
            }
            _vao.Bind();
            _vao.VertexAttributePointer(11, 3, VertexAttribPointerType.Float, 3, 0); // morph position
            _vao.Unbind();
        }

        for (int section = 0; section < Sections.Length; section++)
        {
            Sections[section].Setup(_gl);
        }
    }

    public void Bind(Shader shader)
    {
        if (IsSelected)
        {
            _gl.Enable(EnableCap.StencilTest);
            _gl.StencilFunc(StencilFunction.Always, 1, 0xFF);
        }

        _vao.Bind();
        shader.SetUniform("uMorphTime", MorphTime);
        shader.SetUniform("display_vertex_colors", DisplayVertexColors);
        for (int section = 0; section < Sections.Length; section++)
        {
            Sections[section].Bind(shader, (uint) TransformsCount);
        }
        _vao.Unbind();

        if (IsSelected)
        {
            _gl.StencilFunc(StencilFunction.Always, 0, 0xFF);
            _gl.Disable(EnableCap.StencilTest);
        }
    }

    public void Outline(Shader shader)
    {
        _gl.StencilMask(0x00);
        _gl.Disable(EnableCap.DepthTest);
        _gl.StencilFunc(StencilFunction.Notequal, 1, 0xFF);

        _vao.Bind();
        shader.Use();
        shader.SetUniform("uMorphTime", MorphTime);
        for (int section = 0; section < Sections.Length; section++)
        {
            if (!Sections[section].Show) continue;
            _gl.DrawArraysInstanced(PrimitiveType.Triangles, Sections[section].FirstFaceIndex, Sections[section].FacesCount, (uint) TransformsCount);
        }
        _vao.Unbind();

        _gl.StencilFunc(StencilFunction.Always, 0, 0xFF);
        _gl.Enable(EnableCap.DepthTest);
        _gl.StencilMask(0xFF);
    }

    public void Dispose()
    {
        _ebo.Dispose();
        _vbo.Dispose();
        _matrixVbo.Dispose();
        _vao.Dispose();
        if (HasMorphTargets)
        {
            _morphVbo.Dispose();
            for (var morph = 0; morph < Morphs.Length; morph++)
            {
                Morphs[morph].Dispose();
            }
        }
        for (int section = 0; section < Sections.Length; section++)
        {
            Sections[section].Dispose();
        }
        _gl.DeleteProgram(_handle);
    }
}
