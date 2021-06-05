/*************************************************
   Copyright (c) 2021 Undersoft

   System.IMemberSecurity.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="IMemberSecurity" />.
    /// </summary>
    public interface IMemberSecurity
    {
        #region Methods

        /// <summary>
        /// The CreateToken.
        /// </summary>
        /// <param name="member">The member<see cref="MemberIdentity"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string CreateToken(MemberIdentity member);

        /// <summary>
        /// The GetByToken.
        /// </summary>
        /// <param name="token">The token<see cref="string"/>.</param>
        /// <returns>The <see cref="MemberIdentity"/>.</returns>
        MemberIdentity GetByToken(string token);

        /// <summary>
        /// The GetByUserId.
        /// </summary>
        /// <param name="userId">The userId<see cref="string"/>.</param>
        /// <returns>The <see cref="MemberIdentity"/>.</returns>
        MemberIdentity GetByUserId(string userId);

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="memberIdentity">The memberIdentity<see cref="MemberIdentity"/>.</param>
        /// <param name="encoded">The encoded<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Register(MemberIdentity memberIdentity, bool encoded = false);

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="di">The di<see cref="MemberIdentity"/>.</param>
        /// <param name="ip">The ip<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Register(string name, string key, out MemberIdentity di, string ip = "");

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="token">The token<see cref="string"/>.</param>
        /// <param name="di">The di<see cref="MemberIdentity"/>.</param>
        /// <param name="ip">The ip<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Register(string token, out MemberIdentity di, string ip = "");

        /// <summary>
        /// The VerifyIdentity.
        /// </summary>
        /// <param name="member">The member<see cref="MemberIdentity"/>.</param>
        /// <param name="checkPasswd">The checkPasswd<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool VerifyIdentity(MemberIdentity member, string checkPasswd);

        /// <summary>
        /// The VerifyToken.
        /// </summary>
        /// <param name="member">The member<see cref="MemberIdentity"/>.</param>
        /// <param name="checkToken">The checkToken<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool VerifyToken(MemberIdentity member, string checkToken);

        #endregion
    }
}
