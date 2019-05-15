using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;

namespace SL.Utils
{
    public class SoundTool
    {
        public static void Play(string path)
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = path;
            player.Load();
            player.Play();
        }
    }
}
