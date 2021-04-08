using System;

using System.Drawing;
using System.Drawing.Imaging;

using System.Runtime.InteropServices;


namespace ImgApp.Containers
{




    unsafe class Picture
    {
        public int Width
        { get; set; }
        public int Height
        { get; set; }
        public byte[] Arr
        { get; set; }
        public byte[] DefaultArr
        { get; set; }

        public int stride;
        BitmapData bd;
   

       

        public unsafe Picture(Bitmap InpImg) //Конструктор
        {

            Width = InpImg.Width;

            Height = InpImg.Height;


             bd = null ; 
            try
            {   
                bd= InpImg.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                Arr = new byte[3 * Height * Width];
                DefaultArr = new byte[3 * Height * Width];

                stride = bd.Stride;
                byte* CurrentPosition;
                int k = 0;
                 (byte* Ptr = Arr)
                {
                    fixed (byte* Ptr2 = DefaultArr)
                    {
                        byte* tmp = Ptr,tmp2=Ptr2;

                        for (int i = 0; i < Height; i++)
                        {
                            CurrentPosition = ((byte*)bd.Scan0) + i * bd.Stride;
                            for (int j = 0; j < Width * 3; j++)
                            {
                                *tmp = *(CurrentPosition);
                                tmp++;
                                *tmp2 = *(CurrentPosition);
                                tmp2++;
                                CurrentPosition++;
                            }
                        }
                    }
                }
            }
            //перевод картинки в массив
            finally
            {
                InpImg.UnlockBits(bd);
            }
        
            
        }

        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }



        public unsafe void Multiplication(Picture tmp, string Chanel, int Alph)
        {
            double A = 1.0 - (Alph / 10.0);
            fixed (byte* Ptr = Arr) //подвязывает участок памяти к определенному указателю
            {
                fixed (byte* Ptr2 = tmp.Arr)
                {
                    byte* wrk = Ptr, wrk2 = Ptr2;

                    if (Chanel == "R")
                        for (int i = 2; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp(*(wrk + i) * *(wrk2 + i) * A / 256, 0, 255);
                    else
                     if (Chanel == "G")
                        for (int i = 1; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp(*(wrk + i) * *(wrk2 + i) * A / 256, 0, 255);
                    else
                     if (Chanel == "B")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp(*(wrk + i) * *(wrk2 + i) * A / 256, 0, 255);
                    else
                     if (Chanel == "RG")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp(*(wrk + i + 2) * *(wrk2 + i + 2) * A / 256, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp(*(wrk + i + 1) * *(wrk2 + i + 1) * A / 256, 0, 255);
                        }
                    else
                     if (Chanel == "RB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp(*(wrk + i + 2) * *(wrk2 + i + 2) * A / 256, 0, 255);
                            *(wrk + i) = (byte)Clamp(*(wrk + i) * *(wrk2 + i) * A / 256, 0, 255);
                        }
                    else
                     if (Chanel == "GB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i) = (byte)Clamp(*(wrk + i) * *(wrk2 + i) * A / 256, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp(*(wrk + i + 1) * *(wrk2 + i + 1) * A / 256, 0, 255);
                        }
                    else
                     if (Chanel == "RGB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp(*(wrk + i + 2) * *(wrk2 + i + 2) * A / 256, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp(*(wrk + i + 1) * *(wrk2 + i + 1) * A / 256, 0, 255);
                            *(wrk + i) = (byte)Clamp(*(wrk + i) * *(wrk2 + i) * A / 256, 0, 255);
                        }
                }
            }

        }
        public unsafe void Summ(Picture tmp, string Chanel, int Alph)
        {
            double A = 1.0 - (Alph / 10.0);
            fixed (byte* Ptr = Arr)
            {
                fixed (byte* Ptr2 = tmp.Arr)
                {
                    byte* wrk = Ptr, wrk2 = Ptr2;

                    //+0 blue //+1 green //+2 red
                    if (Chanel == "R")
                        for (int i = 2; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp(*(wrk + i) + *(wrk2 + i) * A, 0, 255);
                    else
                    if (Chanel == "G")
                        for (int i = 1; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp(*(wrk + i) + *(wrk2 + i) * A, 0, 255);
                    else
                    if (Chanel == "B")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp(*(wrk + i) + *(wrk2 + i) * A, 0, 255);
                    else
                    if (Chanel == "RG")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp(*(wrk + i + 2) + *(wrk2 + i + 2) * A, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp(*(wrk + i + 1) + *(wrk2 + i + 1) * A, 0, 255);
                        }
                    else
                    if (Chanel == "RB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp(*(wrk + i + 2) + *(wrk2 + i + 2) * A, 0, 255);
                            *(wrk + i) = (byte)Clamp(*(wrk + i) + *(wrk2 + i) * A, 0, 255);
                        }
                    else
                    if (Chanel == "GB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i) = (byte)Clamp(*(wrk + i) + *(wrk2 + i) * A, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp(*(wrk + i + 1) + *(wrk2 + i + 1) * A, 0, 255);
                        }
                    else
                    if (Chanel == "RGB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp(*(wrk + i + 2) + *(wrk2 + i + 2) * A, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp(*(wrk + i + 1) + *(wrk2 + i + 1) * A, 0, 255);
                            *(wrk + i) = (byte)Clamp(*(wrk + i) + *(wrk2 + i) * A, 0, 255);
                        }




                }
            }


        }
        public unsafe void Difference(Picture tmp, string Chanel,int Alph) //разность
        {
            double A = 1.0 - (Alph / 10.0);
            fixed (byte* Ptr = Arr)
            {
                fixed (byte* Ptr2 = tmp.Arr)
                {
                    byte* wrk = Ptr, wrk2 = Ptr2;

                    if (Chanel == "R")
                        for (int i = 2; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp(*(wrk + i) - *(wrk2 + i) * A, 0, 255);
                    else
                    if (Chanel == "G")
                        for (int i = 1; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp(*(wrk + i) - *(wrk2 + i) * A, 0, 255);
                    else
                    if (Chanel == "B")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp(*(wrk + i) - *(wrk2 + i) * A, 0, 255);
                    else
                    if (Chanel == "RG")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp(*(wrk + i + 2) - *(wrk2 + i + 2) * A, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp(*(wrk + i + 1) - *(wrk2 + i + 1) * A, 0, 255);
                        }
                    else
                    if (Chanel == "RB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp(*(wrk + i + 2) - *(wrk2 + i + 2) * A, 0, 255);
                            *(wrk + i) = (byte)Clamp(*(wrk + i) - *(wrk2 + i) * A, 0, 255);
                        }
                    else
                    if (Chanel == "GB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i) = (byte)Clamp(*(wrk + i) - *(wrk2 + i) * A, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp(*(wrk + i + 1) - *(wrk2 + i + 1) * A, 0, 255);
                        }
                    else
                    if (Chanel == "RGB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp(*(wrk + i + 2) - *(wrk2 + i + 2) * A, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp(*(wrk + i + 1) - *(wrk2 + i + 1) * A, 0, 255);
                            *(wrk + i) = (byte)Clamp(*(wrk + i) - *(wrk2 + i) * A, 0, 255);
                        }
                }
            }

        }
        public unsafe void Avg(Picture tmp, string Chanel, int Alph) //среднее арифмитическое
        {
            double A = 1.0 - (Alph / 10.0);
            fixed (byte* Ptr = Arr)
            {
                fixed (byte* Ptr2 = tmp.Arr)
                {
                    byte* wrk = Ptr, wrk2 = Ptr2;

                    if (Chanel == "R")
                        for (int i = 2; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp((*(wrk + i) + *(wrk2 + i) * A) / 2, 0, 255);
                    else
                    if (Chanel == "G")
                        for (int i = 1; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp((*(wrk + i) + *(wrk2 + i) * A) / 2, 0, 255);
                    else
                    if (Chanel == "B")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Clamp((*(wrk + i) + *(wrk2 + i) * A) / 2, 0, 255);
                    else
                    if (Chanel == "RG")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp((*(wrk + i + 2) + *(wrk2 + i + 2) * A) / 2, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp((*(wrk + i + 1) + *(wrk2 + i + 1) * A) / 2, 0, 255);
                        }
                    else
                    if (Chanel == "RB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp((*(wrk + i + 2) + *(wrk2 + i + 2) * A) / 2, 0, 255);
                            *(wrk + i) = (byte)Clamp((*(wrk + i) + *(wrk2 + i) * A) / 2, 0, 255);
                        }
                    else
                    if (Chanel == "GB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i) = (byte)Clamp((*(wrk + i) + *(wrk2 + i) * A) / 2, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp((*(wrk + i + 1) + *(wrk2 + i + 1) * A) / 2, 0, 255);
                        }
                    else
                    if (Chanel == "RGB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Clamp((*(wrk + i + 2) + *(wrk2 + i + 2) * A) / 2, 0, 255);
                            *(wrk + i + 1) = (byte)Clamp((*(wrk + i + 1) + *(wrk2 + i + 1) * A) / 2, 0, 255);
                            *(wrk + i) = (byte)Clamp((*(wrk + i) + *(wrk2 + i) * A) / 2, 0, 255);
                        }
                }
            }

        }
        public unsafe void Min(Picture tmp, string Chanel,int Alph) //мин
        {
            double A = 1.0 - (Alph / 10.0);
            fixed (byte* Ptr = Arr)
            {
                fixed (byte* Ptr2 = tmp.Arr)
                {
                    byte* wrk = Ptr, wrk2 = Ptr2;

                    if (Chanel == "R")
                        for (int i = 2; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Math.Min(*(wrk + i), *(wrk2 + i) * A);
                    else
                    if (Chanel == "G")
                        for (int i = 1; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Math.Min(*(wrk + i), *(wrk2 + i) * A);
                    else
                    if (Chanel == "B")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Math.Min(*(wrk + i), *(wrk2 + i) * A);
                    else
                    if (Chanel == "RG")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Math.Min(*(wrk + i + 2), *(wrk2 + i + 2) * A);
                            *(wrk + i + 1) = (byte)Math.Min(*(wrk + i + 1), *(wrk2 + i + 1) * A);
                        }
                    else
                    if (Chanel == "RB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Math.Min(*(wrk + i + 2), *(wrk2 + i + 2) * A);
                            *(wrk + i) = (byte)Math.Min(*(wrk + i), *(wrk2 + i) * A);
                        }
                    else
                    if (Chanel == "GB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i) = (byte)Math.Min(*(wrk + i), *(wrk2 + i) * A);
                            *(wrk + i + 1) = (byte)Math.Min(*(wrk + i + 1), *(wrk2 + i + 1) * A);
                        }
                    else
                    if (Chanel == "RGB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Math.Min(*(wrk + i + 2), *(wrk2 + i + 2) * A);
                            *(wrk + i + 1) = (byte)Math.Min(*(wrk + i + 1), *(wrk2 + i + 1) * A);
                            *(wrk + i) = (byte)Math.Min(*(wrk + i), *(wrk2 + i) * A);
                        }
                }
            }
        }
        public unsafe void Max(Picture tmp, string Chanel, int Alph) //максимум
        {
            double A = 1.0 - (Alph / 10.0);
            fixed (byte* Ptr = Arr)
            {
                fixed (byte* Ptr2 = tmp.Arr)
                {
                    byte* wrk = Ptr, wrk2 = Ptr2;

                    if (Chanel == "R")
                        for (int i = 2; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Math.Max(*(wrk + i), *(wrk2 + i) * A);
                    else
                    if (Chanel == "G")
                        for (int i = 1; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Math.Max(*(wrk + i), *(wrk2 + i) * A);
                    else
                    if (Chanel == "B")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                            *(wrk + i) = (byte)Math.Max(*(wrk + i), *(wrk2 + i) * A);
                    else
                    if (Chanel == "RG")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Math.Max(*(wrk + i + 2), *(wrk2 + i + 2) * A);
                            *(wrk + i + 1) = (byte)Math.Max(*(wrk + i + 1), *(wrk2 + i + 1) * A);
                        }
                    else
                    if (Chanel == "RB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Math.Max(*(wrk + i + 2), *(wrk2 + i + 2) * A);
                            *(wrk + i) = (byte)Math.Max(*(wrk + i), *(wrk2 + i) * A);
                        }
                    else
                    if (Chanel == "GB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i) = (byte)Math.Max(*(wrk + i), *(wrk2 + i) * A);
                            *(wrk + i + 1) = (byte)Math.Max(*(wrk + i + 1), *(wrk2 + i + 1) * A);
                        }
                    else
                    if (Chanel == "RGB")
                        for (int i = 0; i < Width * Height * 3; i = i + 3)
                        {
                            *(wrk + i + 2) = (byte)Math.Max(*(wrk + i + 2), *(wrk2 + i + 2) * A);
                            *(wrk + i + 1) = (byte)Math.Max(*(wrk + i + 1), *(wrk2 + i + 1) * A);
                            *(wrk + i) = (byte)Math.Max(*(wrk + i), *(wrk2 + i) * A);
                        }
                }
            }

        }


        

        
     

        public unsafe void ChangeAlphAndChanel(int lvl, string Chanel) //
        {
            double A = 1.0 - (lvl / 10.0);
            fixed (byte* DefaultPtr = DefaultArr)
            {
                fixed (byte* Ptr = Arr)
                {
                    byte* wrk = Ptr,wrk2=DefaultPtr;
                    
                        
                        
                            if (Chanel == "R")
                        for (int i = 0; i < Height * Width * 3; i = i + 3)
                        {
                                *(wrk + i  + 1) = 0;
                                *(wrk + i   ) = 0;
                                *(wrk + i   + 2) = (byte)((*(wrk2 + i + 2)) * A);

                            }
                            else
                           if (Chanel == "G")
                        for (int i = 0; i < Height * Width * 3; i = i + 3)
                        {
                                *(wrk + i    + 2) = 0;
                                *(wrk + i ) = 0;
                                *(wrk + i  + 1) = (byte)((*(wrk2 + i + 1)) * A);
                            }
                            else
                           if (Chanel == "B")
                        for (int i = 0; i < Height * Width * 3; i = i + 3)
                        {
                                *(wrk + i    + 2) = 0;
                                *(wrk + i  + 1) = 0;
                                *(wrk + i  ) = (byte)((*(wrk2 + i )) * A);
                            }
                            else
                            if (Chanel == "RG")
                        for (int i = 0; i < Height * Width * 3; i = i + 3)
                        {
                                *(wrk + i + 2) = (byte)((*(wrk2 + i  + 2)) * A);
                                *(wrk + i + 1) = (byte)((*(wrk2 + i  + 1)) * A);
                                *(wrk + i ) = 0;
                            }
                            else
                            if (Chanel == "RB")
                        for (int i = 0; i < Height * Width * 3; i = i + 3)
                        {
                                *(wrk + i  + 2) = (byte)((*(wrk2 + i + 2)) * A);
                                *(wrk + i  + 1) = 0;
                                *(wrk + i ) = (byte)((*(wrk2 + i )) * A);
                            }
                            else
                       
                            if (Chanel == "GB")
                                for (int i = 0; i < Height * Width * 3; i = i + 3)
                                {
                                *(wrk + i  + 2) = 0;
                                *(wrk + i  + 1) = (byte)((*(wrk2 + i  + 1)) * A);
                                *(wrk + i ) = (byte)((*(wrk2 + i )) * A);
                            }
                            else
                            if(Chanel=="RGB")
                                for (int i = 0; i < Height * Width * 3; i = i + 3)
                                {
                                *(wrk + i  + 2) = (byte)((*(wrk2 + i  + 2)) * A);
                                *(wrk + i  + 1) = (byte)((*(wrk2 + i  + 1)) * A);
                                *(wrk + i ) = (byte)((*(wrk2 + i )) * A);
                            }

                            


                        
                }
            }
        }
        

     

        



        public Bitmap TakePicture()
        {
            
            Bitmap im = new Bitmap(Width, Height, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(Arr, 0));
            //кладу в битман получившийся массив
            Arr = null;
            DefaultArr = null;
            //GC.Collect();
            return im;
        }

    }
}
