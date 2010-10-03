// GenericOption.cs
//
//  Copyright (C) 2006 SharpKnocking project
//  Created by Miguel Angel Pérez, mangelp@gmail.com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
using System;
using System.Collections;
using System.Collections.Specialized;

using NFSharp.Iptables.Core.Extensions;
using NFSharp.Iptables.Core.Extensions.Match;

using CommonUtilities.Types;
using NFSharp.Iptables.Parser.IptablesSaveFormat;
using NFSharp.Iptables.Util;

namespace NFSharp.Iptables.Core.Commands.Options {

    /// <summary>
    /// Models a generic option used in NetfilterRule object
    /// </summary>
    public abstract class GenericOption: NegableParameter {
        //FIXME: Erase this commented block if it is unnecesary
//		/// <summary>
//		/// Returns true if the option is in long format (-- prefix) or false if it
//		/// is in short format (- prefix).
//		/// </summary>
//		/// <value>True of false</value>
//		public override bool IsLongFormat
//		{
//			get
//			{
//				string def = AliasUtil.GetDefaultAlias (this.optionType);
//				if(def!=null && def.Length > 1)
//					return true;
//
//				return false;
//			}
//		}

        private NetfilterRule parentRule;

        /// <summary>
        /// Gets/Sets the parent rule owner of this option
        /// </summary>
        /// <remarks>
        /// If this option has already a owner the rule is removed first from the old
        /// owner but only if it is not the same as the newly asigned one
        /// </remarks>
        public NetfilterRule ParentRule {
            get {
                return this.parentRule;
            } set {
                if(this.parentRule != null && value != null && value != parentRule) {
                    this.parentRule.Options.Remove(this);
                }
                this.parentRule = value;
            }
        }

        private RuleOptions optionType;

        /// <summary>
        /// Gets the option type
        /// </summary>
        protected RuleOptions OptionType {
            get {
                return this.optionType;
            }
        }

        private bool hasImplicitExtension;

        /// <summary>
        /// Gets if the option implicitly loads a extension
        /// </summary>
        /// <remarks>
        /// See iptables man page about -p option with tcp, udp or icp values as they
        /// implicitly causes the loading of the correspondent match extension.
        /// </remarks>
        public bool HasImplicitExtension {
            get {
                return this.hasImplicitExtension;
            }
        }

        private Type extensionType;

        /// <summary>
        /// Returns the type for the implicit extension if this option has
        /// a implicit extension.
        /// </summary>
        /// <returns>
        /// The type if this option has an implicit extension or false if not
        /// </returns>
        public virtual Type ExtensionType {
            get {
                return this.extensionType;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="option">Option type</param>
        /// <param name="implicitExt">Implicit extension type for the option</param>
        public GenericOption(RuleOptions option, MatchExtension implicitEx) {
            this.optionType = option;
            this.SetImplicitExtension(implicitEx);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericOption(RuleOptions option) {
            //This sets everything to defaults
            this.optionType = option;
        }

        /// <summary>
        /// Returns the default alias name for this option
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> with the default alias name for this option
        /// </returns>
        /// <remarks>
        /// Uses the aliases defined in the enumeration with all the known option types.
        /// </remarks>
        public override string GetDefaultAlias() {
            return AliasUtil.GetDefaultAlias (this.optionType);
        }

        /// <summary>
        /// Returns if a name is an alias of the option
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> with the name to check
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/> that says if the name is a valid alias or not
        /// </returns>
        public override bool IsAlias(string name) {
            return AliasUtil.IsAliasName (this.optionType, name);
        }

        /// <summary>
        /// Sets the extension that will be loaded implicitly if the current
        /// option is used.
        /// </summary>
        /// <remarks>
        /// This method will set the flag hasImplicitExtension to true if the
        /// loading of the extension is successfull. If not the value is left
        /// as is.
        /// </remarks>
        protected void SetImplicitExtension(MatchExtension extension) {
            this.extensionType = MatchExtensionFactory.GetExtensionType(extension);

            if(this.extensionType==null)
                throw new InvalidOperationException("Can't load the implementation "+
                                                    "for the extension "+
                                                    extension.ToString().ToLower()+".");
            this.hasImplicitExtension = true;
        }

        //cache for decoding names as options
        private static EnumValueAliasCache optNameCache;

        static GenericOption() {
            optNameCache = new EnumValueAliasCache();
            optNameCache.FillFromEnum (typeof(RuleOptions));
        }

        /// <summary>
        /// Gets if the parameter name is a option
        /// </summary>
        public static bool IsOption(string option) {
            if(optNameCache.Exists(option)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the Options enumeration constant that matches the
        /// option name. The name can be any valid alias for the option but
        /// it can't be the name of a constant of the enumeration.
        /// </summary>
        /// <returns>
        /// Options.None if the name is not a valid alias or it is the name of
        /// a constant in the enumeration.
        /// </returns>
        public static RuleOptions GetOptionType(string optName) {
            if(optNameCache.Exists(optName)) {
                return (RuleOptions)optNameCache[optName];
            }

            return RuleOptions.None;
        }

        /// <summary>
        /// Returns if the name is a valid option.
        /// </summary>
        public static bool IsValidName(string name) {
            if(GenericOption.optNameCache.Exists(name)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to load values for the current option from a string. If fails
        /// an error string is returned by an output parameter
        /// </summary>
        /// <returns>
        /// True if the string can be converted to a set of values for the option
        /// or false if not.
        /// </returns>
        public abstract bool TryReadValues(string strVal, out string errMsg);
    }
}
