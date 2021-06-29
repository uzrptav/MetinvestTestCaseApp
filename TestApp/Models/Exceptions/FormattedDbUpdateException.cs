using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;

namespace TestApp.Models.Exceptions
{
    public class FormattedDbUpdateException : Exception
    {
        public FormattedDbUpdateException(DbUpdateException innerException) :
           base(null, innerException)
        {
        }

        public override string Message
        {
            get
            {
                var innerException = InnerException as DbUpdateException;
                if (innerException != null)
                {
                    StringBuilder sb = new StringBuilder();

                    try
                    {
                        foreach (var result in innerException.Entries)
                        {
                            sb.AppendFormat("Type: {0} cause error. ", result.Entity.GetType().Name);
                        }
                    }
                    catch (Exception e)
                    {
                        sb.Append("Error parsing DbUpdateException: " + e.ToString());
                    }

                    return sb.ToString();
                }

                return base.Message;
            }
        }
    }
}