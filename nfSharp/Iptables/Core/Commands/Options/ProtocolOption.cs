
using System;

using NFSharp.Iptables;

using CommonUtilities.Net;
using CommonUtilities.Types;

using CommonUtilities.Unix.Net;


namespace NFSharp.Iptables.Core.Commands.Options {

    /// <summary>
    /// Models the protocol option
    /// </summary>
    public class ProtocolOption: GenericOption {
        private Protocols protocol;

        public Protocols Protocol {
            get {
                return this.protocol;
            } set {
                this.protocol = value;
            }
        }

        public ProtocolOption()
        :base(RuleOptions.Protocol) {
        }


        public override bool TryReadValues (string strVal, out string errStr) {
            object obj;
            errStr=String.Empty;

            if(AliasUtil.IsAliasName(typeof(Protocols), strVal, out obj)) {
                this.protocol = (Protocols)obj;
                return true;
            }

            errStr = "The value '"+strVal+"' can't be converted to any known protocol";
            return false;
        }

        protected override string GetValueAsString() {
            return AliasUtil.GetDefaultAlias (this.protocol);
        }
    }
}
