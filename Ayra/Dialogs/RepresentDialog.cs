using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ayra.Dialogs {
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    [Serializable]
    public class RepresentDialog : IDialog<object> {
        public async Task StartAsync(IDialogContext context) {
            context.Fail(new NotImplementedException("ops"));
        }
    }
}