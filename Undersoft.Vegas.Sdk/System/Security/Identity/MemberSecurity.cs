/*************************************************
   Copyright (c) 2021 Undersoft

   System.MemberSecurity.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="MemberSecurity" />.
    /// </summary>
    public abstract class MemberSecurity : IMemberSecurity
    {
        #region Methods

        /// <summary>
        /// The CreateToken.
        /// </summary>
        /// <param name="member">The member<see cref="MemberIdentity"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public virtual string CreateToken(MemberIdentity member)
        {
            string token = null;
            string key = member.Key;
            string timesalt = Convert.ToBase64String(DateTime.Now.Ticks.ToString().ToBytes(CharEncoding.ASCII));
            token = CryptoHashKey.Encrypt(key, 1, timesalt);
            member.Token = token;
            DateTime time = DateTime.Now;
            member.RegisterTime = time;
            member.LifeTime = time.AddMinutes(30);
            member.LastAction = time;
            return token;
        }

        /// <summary>
        /// The GetByToken.
        /// </summary>
        /// <param name="token">The token<see cref="string"/>.</param>
        /// <returns>The <see cref="MemberIdentity"/>.</returns>
        public abstract MemberIdentity GetByToken(string token);

        /// <summary>
        /// The GetByUserId.
        /// </summary>
        /// <param name="userId">The userId<see cref="string"/>.</param>
        /// <returns>The <see cref="MemberIdentity"/>.</returns>
        public abstract MemberIdentity GetByUserId(string userId);

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="memberIdentity">The memberIdentity<see cref="MemberIdentity"/>.</param>
        /// <param name="encoded">The encoded<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public abstract bool Register(MemberIdentity memberIdentity, bool encoded = false);

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="di">The di<see cref="MemberIdentity"/>.</param>
        /// <param name="ip">The ip<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public abstract bool Register(string name, string key, out MemberIdentity di, string ip = "");

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="token">The token<see cref="string"/>.</param>
        /// <param name="di">The di<see cref="MemberIdentity"/>.</param>
        /// <param name="ip">The ip<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public abstract bool Register(string token, out MemberIdentity di, string ip = "");

        /// <summary>
        /// The VerifyIdentity.
        /// </summary>
        /// <param name="member">The member<see cref="MemberIdentity"/>.</param>
        /// <param name="checkPasswd">The checkPasswd<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public virtual bool VerifyIdentity(MemberIdentity member, string checkPasswd)
        {
            bool verify = false;

            string hashpasswd = member.Key;
            string saltpasswd = member.Salt;
            verify = CryptoHashKey.Verify(hashpasswd, saltpasswd, checkPasswd);

            return verify;
        }

        /// <summary>
        /// The VerifyToken.
        /// </summary>
        /// <param name="member">The member<see cref="MemberIdentity"/>.</param>
        /// <param name="checkToken">The checkToken<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public virtual bool VerifyToken(MemberIdentity member, string checkToken)
        {
            bool verify = false;

            string token = member.Token;

            if (checkToken.Equals(token))
            {
                DateTime time = DateTime.Now;
                DateTime registerTime = member.RegisterTime;
                DateTime lastAction = member.LastAction;
                DateTime lifeTime = member.LifeTime;
                if (lifeTime > time)
                    verify = true;
                else if (lastAction > time.AddMinutes(-30))
                {
                    member.LifeTime = time.AddMinutes(30);
                    member.LastAction = time;
                    verify = true;
                }
            }
            return verify;
        }

        #endregion
    }
}
