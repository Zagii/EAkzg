using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAkzg_wyrzyg_task
{
    class Program
    {
        static void Main(string[] args)
        {
            EAkzg_WindowsService.EA_APISerwis e = new EAkzg_WindowsService.EA_APISerwis();
            string projekt = "";
            if (args.Length>0)
            {
                if (args[0].Length > 0) 
                    projekt = args[0]; 
            }

            Console.WriteLine("EAkzg_wyrzyg_task start : " + projekt);
            e.dzialajDlaWszystkich(projekt);
            Console.WriteLine("EAkzg_wyrzyg_task koniec : " + projekt);
            return;
        }
    }
}
