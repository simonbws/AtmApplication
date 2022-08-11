using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmApplication.UI
{
    public static class Validation
    {
        public static T Convert<T>(string prompt)
        {
            bool valid = false;
            string userInput;

            while(!valid)
            {
                userInput = Utility.GetUserInput(prompt);

                try //convert into the specified type
                {
                    var convertion = TypeDescriptor.GetConverter(typeof(T));
                    if(convertion != null)
                    {
                        return (T)convertion.ConvertFromString(userInput);
                    }
                    else
                    {
                        return default;
                    }
                }
                catch
                {
                    Utility.TypeMessage("Wrong number.Please do it again.", false);
                }
            }
            return default;
        }
    }
}
