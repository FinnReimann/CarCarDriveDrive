using UnityEngine;

public class PixelLineReader : MonoBehaviour
{
    [SerializeField] private new Camera camera; // Die Unity-Kameras, deren Bild analysiert werden soll
    [SerializeField] private Color color;
    
    private void Start()
    {
        FindRedLines();
    }

    private void FindRedLines()
    {
        int screenWidth = camera.pixelWidth;
        int screenHeight = camera.pixelHeight;
        
        
        // Erstelle ein Textur-Objekt und kopiere das Kamerabild darauf
        Texture2D pixelImage = new Texture2D(screenWidth, screenHeight);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
        pixelImage.ReadPixels(new Rect(0, 0, screenWidth, screenHeight), 0, 0);
        pixelImage.Apply();
        RenderTexture.active = currentRT;
        
        Color[] pixels = pixelImage.GetPixels();

        for (int y = 0; y < screenHeight; y++)
        {
            for (int x = 0; x < screenWidth; x++)
            {

                Debug.Log(pixels[y * screenWidth + x]);

            }
        }

        // Suchen nach horizontalen, vertikalen und diagonalen Linien aus roten Pixeln
        for (int y = 0; y < screenHeight; y++)
        {
            for (int x = 0; x < screenWidth; x++)
            {
                Color pixelColor = pixels[y * screenWidth + x];

                // Überprüfe, ob der Farbwert der roten Linie entspricht
                if (IsRedPixel(pixelColor))
                {
                    // Überprüfe horizontale Linie
                    bool horizontalLine = CheckHorizontalLine(pixels, screenWidth, screenHeight, x, y);

                    // Überprüfe vertikale Linie
                    bool verticalLine = CheckVerticalLine(pixels, screenWidth, screenHeight, x, y);

                    // Überprüfe diagonale Linie (links oben nach rechts unten)
                    bool diagonalLine = CheckDiagonalLine(pixels, screenWidth, screenHeight, x, y, 1, 1);

                    // Überprüfe diagonale Linie (rechts oben nach links unten)
                    diagonalLine |= CheckDiagonalLine(pixels, screenWidth, screenHeight, x, y, -1, 1);

                    // Überprüfe, ob eine Linie gefunden wurde
                    if (horizontalLine || verticalLine || diagonalLine)
                    {
                        Debug.Log("Rote Linie gefunden bei X=" + x + ", Y=" + y);
                    }
                    else
                    {
                        Debug.Log("Keine Linie Gefunden!");
                    }
                }
                else
                {
                    Debug.Log(pixelColor);
                }
            }
        }
    }

    private bool CheckHorizontalLine(Color[] pixels, int width, int height, int startX, int y)
    {
        int endX = startX;

        // Finde das Ende der horizontalen Linie
        while (endX < width)
        {
            Color pixelColor = pixels[y * width + endX];

            if (!IsRedPixel(pixelColor))
            {
                break;
            }

            endX++;
        }

        // Überprüfe, ob die Linie lang genug ist
        return (endX - startX) >= 2;
    }

    private bool CheckVerticalLine(Color[] pixels, int width, int height, int x, int startY)
    {
        int endY = startY;

        // Finde das Ende der vertikalen Linie
        while (endY < height)
        {
            Color pixelColor = pixels[endY * width + x];

            if (!IsRedPixel(pixelColor))
            {
                break;
            }

            endY++;
        }

        // Überprüfe, ob die Linie lang genug ist
        return (endY - startY) >= 2;
    }

    private bool CheckDiagonalLine(Color[] pixels, int width, int height, int startX, int startY, int stepX, int stepY)
    {
        int endX = startX;
        int endY = startY;

        // Finde das Ende der diagonalen Linie
        while (endX >= 0 && endX < width && endY >= 0 && endY < height)
        {
            Color pixelColor = pixels[endY * width + endX];

            if (!IsRedPixel(pixelColor))
            {
                break;
            }

            endX += stepX;
            endY += stepY;
        }

        // Überprüfe, ob die Linie lang genug ist
        return Mathf.Abs(endX - startX) >= 2 || Mathf.Abs(endY - startY) >= 2;
    }

    private bool IsRedPixel(Color pixelColor)
    {
        // Überprüfe den Farbwert auf Rot
        float threshold = 0.9f;
        return pixelColor.r >= threshold && pixelColor.g < threshold && pixelColor.b < threshold;
    }
}

