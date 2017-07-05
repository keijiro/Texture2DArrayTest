using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Tester : MonoBehaviour
{
    [SerializeField, HideInInspector] ComputeShader _writerShader;
    [SerializeField, HideInInspector] Shader _readerShader;

    Material _readerMaterial;
    RenderTexture _textureArray;

    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            if (_readerMaterial != null) Destroy(_readerMaterial);
            if (_textureArray != null) Destroy(_textureArray);
        }
        else
        {
            if (_readerMaterial != null) DestroyImmediate(_readerMaterial);
            if (_textureArray != null) DestroyImmediate(_textureArray);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_readerMaterial == null)
        {
            _readerMaterial = new Material(_readerShader);
            _readerMaterial.hideFlags = HideFlags.DontSave;
        }

        if (_textureArray == null)
        {
            _textureArray = new RenderTexture(8, 8, 0, RenderTextureFormat.ARGBHalf);
            _textureArray.hideFlags = HideFlags.DontSave;
            _textureArray.enableRandomWrite = true;
            _textureArray.dimension = TextureDimension.Tex2DArray;
            _textureArray.volumeDepth = 8;
            _textureArray.Create();
        }

        var kernel = _writerShader.FindKernel("WriterMain");
        _writerShader.SetTexture(kernel, "TextureArray", _textureArray);
        _writerShader.Dispatch(kernel, 1, 1, 1);

        _readerMaterial.SetTexture("_TextureArray", _textureArray);
        Graphics.Blit(null, destination, _readerMaterial, 0);
    }
}
