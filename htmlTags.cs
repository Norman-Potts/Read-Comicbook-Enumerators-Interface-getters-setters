/*  
    Lab4b:   A Tangled Web
    Author: Norman Lawerence Potts
    Date: March 22rd 2017    
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Lab4b
{


    /// <summary>
    /// Simple class for an html tag.
    /// </summary>
    class htmlTags 
    {
        public string tag { get; private set; } = "";                

        /// <summary>
        ///  Constructor removes attributes after the first space in the reciving string and saves it as a lower case.
        /// </summary>
        /// <param name="gentag"></param>        
        public htmlTags(string gentag )
        {
            // Remove attibutes after the first space.
            string[] cut = gentag.Split(' ');
            string t = cut[0];
            /// Save as lower case.
            tag = t.ToLower();                    
        }


        /// <summary>
        /// Determines if the closing tag matchs this tag.
        /// </summary>
        /// <returns>True if they match.</returns>
        public bool Pairs( htmlTags Closetags ) {
            bool theyMatch = false;
            /// Remove '/' character.
            string t = Closetags.tag;
            Char[] cut = t.ToCharArray();
            string removed = ""; 
            for (int i = 0; i < cut.Length; i++) {
                if (cut[i] != '/') {
                    removed = removed + cut[i];
                }
            }            
            if (this.tag == removed) {
                theyMatch = true;
            } else {
                
                theyMatch = false;
            }        
            return theyMatch;
        }
        
    }
}
