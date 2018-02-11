using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetPerformance
{
    /// <summary>
    /// Static Helper class to sanitize and keep a list of all of the instance names that have been used
    /// </summary>
    /// <remarks>
    /// <para>
    /// Per the MSDN documentation, instance names are limited to 128 characters in length.  It is 
    /// possible that when concatenating all of the elements together to form the instance name, we
    /// could end up with a name longer than 128 characters.  In this case, we'll take the first 125
    /// characters and then append on a 3 digits of an integer.  (So there is a limit of 999 names
    /// that exceed 128 characters, but this really should not be an issue).  All of the instance 
    /// names are then kept in a dictionary, with the raw (original) instance name as the key
    /// and the sanitized name as the value so on subsequent calls, the action method gets sanitized
    /// to the same name
    /// </para>
    /// </remarks>
    public static class InstanceNameRegistry
    {
        private static Dictionary<string, string> instanceNames = new Dictionary<string, string>();
        private static Object lockObject = new Object();
        private static int methodCounter = 0;

        /// <summary>
        /// Sanitizes the instance name to make sure it is 128 characters or less
        /// </summary>
        /// <param name="rawInstanceName">A Stirng of the raw (original) instance name</param>
        /// <returns>A string of the instance name that should be used</returns>
        public static string GetSanitizedInstanceName(string rawInstanceName)
        {
            // If this is a known instance, then we can find it and just return.  No need to lock
            if (instanceNames.ContainsKey(rawInstanceName))
            {
                return instanceNames[rawInstanceName];
            }

            lock (lockObject)
            {
                // Check again, in case someone added it while we were waiting
                if (instanceNames.ContainsKey(rawInstanceName) == false)
                {
                    if (rawInstanceName.Length <= 128)
                    {
                        // No need to truncate the method name, so just add it
                        instanceNames.Add(rawInstanceName, rawInstanceName);
                    }
                    else
                    {
                        // Need to get this below 128 chars.  
                        // so take the first 125, and then add on an integer
                        string sanitizedName = string.Format("{0}{1:###}",
                            rawInstanceName.Substring(0, 125), methodCounter++);
                        instanceNames.Add(rawInstanceName, sanitizedName);
                    }
                }
            }
            return instanceNames[rawInstanceName];
        }

        /// <summary>
        /// Gets a list of all of the instance names that have been used
        /// </summary>
        /// <remarks>
        /// This method is really only intended to be used when the application ends
        /// and you want to remove the instances from the performance framework
        /// </remarks>
        /// <returns>A List of strings of the instance names</returns>
        public static List<string> GetAllInstanceNames() => instanceNames.Values.ToList();
    }
}
