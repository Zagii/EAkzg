using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAkzg
{
    static class txt
    {
        static public String dajTekst(String s)
        {
            String w = "";
            switch (s)
            {
                case "h1.kzg":
                    w = "<h1><a id=\"^@^\">^@^</a><a class=\"spisPowrot\" href=\"#spisTresci\">Powrót do spisu treści </a></h1>";
                    break;
                case "h2.kzg":
                    w = "<h2><a id=\"^@^\">^@^</a><a class=\"spisPowrot\" href=\"#spisTresci\">Powrót do spisu treści </a></h2>";
                    break;
                case "h3.kzg":
                    w = "<h3><a id=\"^@^\">^@^</a><a class=\"spisPowrot\" href=\"#spisTresci\">Powrót do spisu treści </a></h3>";
                    break;
                case "wstep.kzg":
                    w= "<img src='^@^img/logo.png'>\n<h1 class=\"tytul\">High Level Design dla projektu <br> ^@^ </h1><br>\n";
                    w+="<ul>\n<li> Autor: ^@^</li>\n<li> Data generowania ^@^ </li>\n<li> Wersja generatora: 0.1,";
                    w+=" wszelkie uwagi dotyczące generatora HLD kierować na adres Krzysztof.Zagawa@exteranal.t-mobile.pl </li>\n";
                    w += " </ul>";
                    break;
                case "naglowek.kzg":
                    w += "<!DOCTYPE html>\n";
                     w += "<html>\n";
                     w += "<head>\n";
                     w += "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\" >\n";
                     w += "<link rel=\"stylesheet\" type=\"text/css\" href=\"css/styl.css\">\n";
                     w += "<title>Dokumentacha High Level Design (HLD) dla projektu ^@^  wygenerowano  ^@^</title>\n";
                     w += "</head>";
                    break;
                case "stopka.kzg":
                    w += " \n</body>\n</html>";
                    break;
                case "rozdzial1.kzg":
                     w+="<p>\n";
                     w+="Celem niniejszego dokumentu jest przedstawienie sposobu realizacji Wymagań Biznesowych dla projektu zawartych w dokumencie Concept Paper. Na opis sposób realizacji składają się następujące główne elementy:<br>\n";
                     w+="1.	odniesienie do wymagań biznesowych<br>\n";
                     w+="2.	zarys koncepcji rozwiązania<br>\n";
                     w+="3.	opis architektury rozwiązania wraz z dekompozycją koniecznych zmian funkcjonalnych na poszczególne systemy<br>\n";
                     w+="4.	opis koniecznych do wykonania zmian w poszczególnych systemach<br>\n";
                     w+="5.	opis zmian koniecznych z punktu widzenia Infrastruktury<br>\n";
                     w+="Zawarte w dokumencie informacje będą podstawą do:\n";
                     w+="<ul>\n";
                     w+="<li>	ustalenia kosztów oraz ostatecznych terminów wdrożenia przedsięwzięcia i tym samym podjęcia decyzji o jego realizacji,</li>\n";
                     w+="<li> 	dalszych prac nad projektem - projektowania spójnego rozwiązania w poszczególnych systemach</li>\n";
                     w+="</ul>\n";
                     w+="</p>\n";
                    break;
            }
            return w;
        }
    }
}
