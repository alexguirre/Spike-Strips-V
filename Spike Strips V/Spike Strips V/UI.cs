#if DEBUG
namespace Spike_Strips_V
{
    using Rage;
    using Rage.Native;
    using System;
    using System.Drawing;


    public class ResText : Text
    {
        public ResText(string caption, Point position, float scale)
            : base(caption, position, scale)
        {
            TextAlignment = Alignment.Left;
        }

        public ResText(string caption, Point position, float scale, Color color)
            : base(caption, position, scale, color)
        {
            TextAlignment = Alignment.Left;
        }

        public ResText(string caption, Point position, float scale, Color color, EFont font, Alignment justify)
            : base(caption, position, scale, color, font, false)
        {
            TextAlignment = justify;
        }


        public Alignment TextAlignment { get; set; }
        public bool DropShadow { get; set; }
        public bool Outline { get; set; }

        /// <summary>
        /// Push a long string into the stack.
        /// </summary>
        /// <param name="str"></param>
        public static void AddLongString(string str)
        {
            const int strLen = 99;
            for (int i = 0; i < str.Length; i += strLen)
            {
                string substr = str.Substring(i, Math.Min(strLen, str.Length - i));
                NativeFunction.CallByHash<uint>(0x6c188be134e074aa, substr);      // _ADD_TEXT_COMPONENT_STRING
            }
        }


        public static float MeasureStringWidth(string str, EFont font, float scale)
        {
            int screenw = Game.Resolution.Width;
            int screenh = Game.Resolution.Height;
            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            float width = height * ratio;
            return MeasureStringWidthNoConvert(str, font, scale) * width;
        }

        public static float MeasureStringWidthNoConvert(string str, EFont font, float scale)
        {
            NativeFunction.CallByHash<ulong>(0x54ce8ac98e120cab, "STRING");
            AddLongString(str);
            return NativeFunction.CallByHash<float>(0x85f061da64ed2f67, (int)font) * scale;
        }

        public Size WordWrap { get; set; }

        public override void Draw(Size offset)
        {
            int screenw = Game.Resolution.Width;
            int screenh = Game.Resolution.Height;

            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            var width = height * ratio;

            float x = (Position.X) / width;
            float y = (Position.Y) / height;
            
            NativeFunction.CallByName<uint>("SET_TEXT_FONT", (int)FontEnum);
            NativeFunction.CallByName<uint>("SET_TEXT_SCALE", 1.0f, Scale);
            NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", (int)Color.R, (int)Color.G, (int)Color.B, (int)Color.A);

            if (DropShadow)
                Rage.Native.NativeFunction.CallByName<uint>("SET_TEXT_DROP_SHADOW");
            if (Outline)
                Rage.Native.NativeFunction.CallByName<uint>("SET_TEXT_OUTLINE");
            switch (TextAlignment)
            {
                case Alignment.Centered:
                    NativeFunction.CallByName<uint>("SET_TEXT_CENTRE", true);
                    break;
                case Alignment.Right:
                    NativeFunction.CallByName<uint>("SET_TEXT_RIGHT_JUSTIFY", true);
                    NativeFunction.CallByName<uint>("SET_TEXT_WRAP", 0, x);
                    break;
            }

            if (WordWrap != new Size(0, 0))
            {
                float xsize = (Position.X + WordWrap.Width) / width;
                NativeFunction.CallByName<uint>("SET_TEXT_WRAP", x, xsize);
            }

            NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "jamyfafi");      // _SET_TEXT_ENTRY
            AddLongString(Caption);


            NativeFunction.CallByHash<uint>(0xcd015e5bb0d96a57, x, y);     // _DRAW_TEXT
        }

        public enum Alignment
        {
            Left,
            Centered,
            Right,
        }
    }


    public class Text
    {
        public float Scale { get; set; }
        public string Caption { get; set; }
        public bool Centered { get; set; }
        public EFont FontEnum { get; set; }
        public Font Font { get { return new Font(FontEnum.ToString(), Scale); } }
        public virtual bool Enabled { get; set; }
        public virtual Point Position { get; set; }
        public virtual Color Color { get; set; }

        public Text(string caption, Point position, float scale)
        {
            this.Enabled = true;
            this.Caption = caption;
            this.Position = position;
            this.Scale = scale;
            this.Color = Color.WhiteSmoke;
            this.FontEnum = EFont.ChaletLondon;
            this.Centered = false;
        }

        public Text(string caption, Point position, float scale, Color color)
        {
            Enabled = true;
            Caption = caption;
            Position = position;
            Scale = scale;
            Color = color;
            FontEnum = EFont.ChaletLondon;
            Centered = false;
        }

        public Text(string caption, Point position, float scale, Color color, EFont font, bool centered)
        {
            Enabled = true;
            Caption = caption;
            Position = position;
            Scale = scale;
            Color = color;
            FontEnum = font;
            Centered = centered;
        }

        public virtual void Draw()
        {
            Draw(new Size());
        }

        public virtual void Draw(Size offset)
        {
            if (!this.Enabled) return;

            float x = (this.Position.X + offset.Width) / 1280;
            float y = (this.Position.Y + offset.Height) / 720;

            NativeFunction.CallByName<uint>("SET_TEXT_FONT", (int)this.FontEnum);
            NativeFunction.CallByName<uint>("SET_TEXT_SCALE", this.Scale, this.Scale);
            NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", (int)this.Color.R, (int)this.Color.G, (int)this.Color.B, (int)this.Color.A);
            NativeFunction.CallByName<uint>("SET_TEXT_CENTRE", this.Centered);
            NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "STRING"); // SetTextEntry native
            NativeFunction.CallByHash<uint>(0x6c188be134e074aa, this.Caption); // AddTextComponentString native
            NativeFunction.CallByHash<uint>(0xcd015e5bb0d96a57, x, y); // DrawText native
        }
    }

    public enum EFont
    {
        ChaletLondon = 0,
        HouseScript = 1,
        Monospace = 2,
        ChaletComprimeCologne = 4,
        Pricedown = 7
    }
}
#endif