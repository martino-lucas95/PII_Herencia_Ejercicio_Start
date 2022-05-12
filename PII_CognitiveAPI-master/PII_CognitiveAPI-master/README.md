![UCU](https://github.com/ucudal/PII_Conceptos_De_POO/raw/master/Assets/logo-ucu.png)

## FIT - Universidad Católica del Uruguay

### Programación II

# API de Reconocimiento Facial

Esta libreria utiliza el servico de _face detection_ de Cognitive Services (https://azure.microsoft.com/en-us/services/cognitive-services/face/) para encontrar caras en las fotos.

Para ello, se utiliza la clase ```CognitiveFace```, la cual recibe por parametro el path a una imagen  y la envia a traves de una llamada REST a la API para detectar si la misma contiene o no una cara.

En caso de encotrar una cara, la propiedad ```FaceFound``` de esa clase tomará el valor ```true```, de lo contrario tendrá el valor ```false```. Si además la propiedad ```MarkFaces``` es ```true```, la librería almacenará una nueva copia de la imagen bajo el nombre tmpFace.jpg con un recuadro dibujado alrededor del rostro encontrado.

Adicionalmente, si se detecta una sonrisa en la cara encontrada, esto se indicará cambiando la propiedad ```SmileFound``` a ```true``` .

## Ejemplo de uso:

```c#
static void Main(string[] args)
{
    CognitiveFace cog = new CognitiveFace(true, Color.GreenYellow);
    cog.Recognize(@"bill.jpg");
    FoundFace(cog);
    cog.Recognize(@"yacht.jpg");
    FoundFace(cog);
    cog.Recognize(@"bill2.jpg");
    FoundFace(cog);
}

static void FoundFace(CognitiveFace cog)
{
    if (cog.FaceFound)
    {
        Console.WriteLine("Face Found!");
        if (cog.SmileFound)
        {
            Console.WriteLine("Found a Smile :)");
        }
        else
        {
            Console.WriteLine("No smile found :(");
        }
    }
    else
        Console.WriteLine("No Face Found");
}
```

> Los repositorios que usan esta librería asumen que fueron descargados en la misma carpeta 'madre'.