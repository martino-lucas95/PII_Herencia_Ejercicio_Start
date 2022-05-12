using System;
using System.Drawing;

namespace CognitiveCoreUCU
{
    class Program
    {
        static void Main(string[] args)
        {
            CognitiveFace cog = new CognitiveFace(false);
            cog.Recognize(@"bill.jpg");
            FoundFace(cog);
            cog.Recognize(@"bill2.jpg");
            FoundFace(cog);
            cog.Recognize(@"yacht.jpg");
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
    }
}
