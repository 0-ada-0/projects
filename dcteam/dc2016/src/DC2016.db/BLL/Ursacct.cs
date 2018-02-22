using System;
using System.Collections.Generic;
using System.Linq;
using DC2016.Model;

namespace DC2016.BLL
{
    public partial class Ursacct
    {
        public static bool IsExistNumber(int number)
        {
            return Select.WhereAcctNumber(number).Count() > 0;
        }
    }

    public partial class UrsacctSelectBuild
    {
        //public UrsacctSelectBuild WhereAcctEMail(params string[] AcctEMail)
        //{
        //    return this.Where1Or("a.`AcctEMail` = {0}", AcctEMail);
        //}
    }
}