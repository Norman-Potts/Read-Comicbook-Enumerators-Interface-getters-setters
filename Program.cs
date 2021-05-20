/*  
    Lab4b:   A Tangled Web
    Author: Norman Lawerence Potts
    Date: March 22rd 2017    
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab4b
{

    /// <summary>
    ///  Lets the user specify a HTML file, then trys to read HTML as a text file, 
    ///  and determines if each opening tag has a corresponding closing tag.It
    ///  then tell the user whether or not the tags are balanced.The program
    ///  will ignore tags that are not container tags.
    /// </summary>
    class Program
    {


        /// <summary>
        /// Handles program startup.  Displays header once then asks user for the name of an html file until one is found. After that, it begins to analyse the file.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string requestedFile;
            Boolean firstLoop = true;
            Boolean foundit = true;
            /// First loop display header. 
            /// Get user input for specified file, if it is found end loop then analyse the html file.
            do
            {
                if (firstLoop == true)
                {
                    Console.WriteLine("___________________________________________________");
                    Console.WriteLine("             Lab4b: A Trangled Web.");
                    Console.WriteLine("___________________________________________________");
                    Console.WriteLine("");
                    Console.WriteLine(" A Simple file checker");
                }
                firstLoop = false;
                Console.WriteLine("");
                Console.Write(" Please enter the name of a html file to analyse:");
                string rawInput = "";
                rawInput = Console.ReadLine();
                requestedFile = rawInput;
                string location = "HTMLfiles/" + requestedFile;
                try
                {   /// If a FileStream object and StreamReader object can be made the file can be found.
                    FileStream file = new System.IO.FileStream(location, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(file);
                    foundit = true;
                }
                catch
                {
                    Console.WriteLine("That html file could not be found!\n");
                    foundit = false;
                }
            } while (foundit == false);
            /// Loop ends when input is a file in HTML files.
            analyseHtmlfile(requestedFile);
            Console.ReadLine();
        }





        /// <summary>
        ///     Creates a stack of htmlTags and determines which tags are opened and closed or single tag type.
        /// 
        ///     The pseudo code for using a stack to analyise an html file.
        ///     
        ///     Initialize boolean for if file is unbalanced to true.
        ///            
        ///     For each tag in file...
        ///         if it is not a self closing tag...
        ///             if it is an opening tag than,
        ///                 Add the opening tag to the stack.
        ///             else if it is a closing tag...        
        ///                 Initalize boolean for finding match set to false.
        ///                 LOOP threw stack
        ///                     Pop tag off top of stack,
        ///                     if popped tag does NOT match the closing tag than,
        ///                         html file is un balanced.  Set that flag to false.
        ///                     other wise if popped tag does match the closing tag than,
        ///                         End loop, and set flag for found match to true.
        ///                 When loop is done,
        ///                 Check flag for if match has NOT been found.
        ///                     html file is un balanced. Set that flag to false.
        ///         
        /// </summary>
        /// <param name="requestedFile">the raw input string given from the user.</param>
        private static void analyseHtmlfile(string requestedFile)
        {

            /// Create Stack and read each line of the html file.
            /// For each line, find the matches for an open tag and add them to the stack of tags.
            /// Ignore tags that are self closing tags.
            Stack<htmlTags> Opentags = new Stack<htmlTags>();
            string location = "HTMLfiles/" + requestedFile;
            string line; int countOfOpenTags = 0;
            FileStream file = new System.IO.FileStream(location, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);

            string anyTag = @"</?[a-z]+[0-9]*[^>]*";            /// Match for any tag....
            Regex regAnyTag = new Regex(anyTag, RegexOptions.IgnoreCase);

            string opentag = @"<[a-z]+[0-9]*[^>]*";            /// Match for a open tag...
            Regex regopentag = new Regex(opentag, RegexOptions.IgnoreCase);

            string closetag = @"</?[a-z]+[0-9]*[^>]*";            /// Match for a open tag...
            Regex regCloseTag = new Regex(closetag, RegexOptions.IgnoreCase);

            string nonContainerPattern = @"<((img)|(hr)|(br))[^>]*";   /// Match for self closing tags that we are ignoring.
            Regex singleReg = new Regex(nonContainerPattern, RegexOptions.IgnoreCase);

            bool balanced = true;

            while ((line = reader.ReadLine()) != null)
            {
                MatchCollection matForAnyTag = regAnyTag.Matches(line);

                /// If there is a match for any kind of tag... 
                if (matForAnyTag.Count > 0)
                {
                    foreach (Match anytag in matForAnyTag)
                    {
                        ///If it isnt a self closing tag...
                        MatchCollection mForSelfClosing = singleReg.Matches(anytag.Value);
                        if (mForSelfClosing.Count == 0)
                        {

                            /// If this tag is an opening tag, else if match for closing tag. 
                            MatchCollection matForOpenTag = regopentag.Matches(anytag.Value);
                            MatchCollection matForCloseTag = regCloseTag.Matches(anytag.Value);
                            if (matForOpenTag.Count > 0)
                            {
                                /// Push open tag to stack.
                                htmlTags tg = new htmlTags(anytag.Value);
                                Opentags.Push(tg);

                            }
                            else if (matForCloseTag.Count > 0)
                            {
                                bool matchFound = false;
                                htmlTags currentClosedTag = new htmlTags(anytag.Value);
                                for (int i = 0; i < Opentags.Count; i++)
                                {
                                    htmlTags poppedTag = Opentags.Pop();                                   
                                    /// If poppedTag is an opening tag that doesn't match current closed tag than file is corrupt.
                                    if (!poppedTag.Pairs(currentClosedTag))
                                    {
                                        /// Currupt!!!
                                        balanced = false;
                                    }
                                    else
                                    {
                                        /// If poppedTag is matches the closing tag than exit loop with a flag saying you found the pair.
                                        matchFound = true;
                                        i = Opentags.Count + 3;/// Ends for loop.
                                    }
                                }
                                /// If the stack gets emptied and there is still not a pair for the current closing tag than the html file is corrupt.
                                if (!matchFound)
                                {
                                    /// Corrupt!!
                                    balanced = false;
                                }
                            }
                        }//End of if selfclosing
                    }/// End of for each match of tag.
                }
            }/// End of while loop for text file read.


            if (Opentags.Count > 0 || balanced == false)
            {
                foreach (htmlTags tag in Opentags)
                {
                    Console.WriteLine("TAG: " + tag.tag);
                }/// End of for each tag.           
            }
            Console.WriteLine("Balance is " + balanced);

        }
    }
        
}
