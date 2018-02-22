using System;
using System.Collections.Generic;
using System.Linq;
using DC2016.Model;

namespace DC2016.BLL {

    public partial class Ursuser
    {
        public static bool CreateUser(int number, string acct, int qq, string mobile, string idcard, string extinfo)
        {
            if (number < 10000)
            {
                return false;
            }
            bool isexisted = Ursacct.IsExistNumber(number);
            UrsacctInfo acctInfo = new UrsacctInfo()
            {
                AcctNumber = number,
                AcctEMail = acct
            };
            Ursacct.Insert(acctInfo);

            if (!isexisted)
            {
                //rc &= URSUserInfo.Create(Operator, ACCT_Number, USRI_QQ, USRI_Mobile, USRI_IDCard);
                UrsuserInfo userInfo = new UrsuserInfo()
                {
                    UrsNumber = number,
                    UrsQQ = qq,
                    UrsMobile = mobile,
                    UrsIDCard = idcard,
                    UrsBirthDay = null,
                    UrsTime = null
                };
                Ursuser.Insert(userInfo);

                //rc &= URSUserExtInfo.Create(Operator, ACCT_Number, UEXT_CHTInfo);
                UrsuserextInfo userExtInfo = new UrsuserextInfo()
                {
                    ExtNumber = number,
                    ExtCHTInfo = extinfo
                };
                Ursuserext.Insert(userExtInfo);
            }
            return true;
        }
    }

	public partial class UrsuserSelectBuild
    {

	}
}