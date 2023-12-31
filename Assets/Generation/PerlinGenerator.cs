using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    [SerializeField] private int pixWidth;
    [SerializeField] private int pixHeight;
    [Space]

    // The origin of the sampled area in the plane.
    private float xOrg;
    private float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    [SerializeField] private float scale = 1.0F;
    [Space]

    [SerializeField] private UnityTexture2DEvent OnPerlinGenerated;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;

        FindPerlinLocation();
    }

    private void FindPerlinLocation()
    {
        bool locationFound = false;
        while (!locationFound)
        {
            xOrg = Random.Range(-10000, 10000);
            yOrg = Random.Range(-10000, 10000);

            locationFound = true;
        }

        CalcNoise();
    }

    private void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0;
        while (y < noiseTex.height)
        {
            float x = 0;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();

        OnPerlinGenerated?.Invoke(noiseTex);
    }
}