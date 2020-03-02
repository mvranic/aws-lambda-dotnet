using System;

namespace AplHandlers {
    public class AplHandler : AplHandlerBase  {
        public AplHandler() { }

        public void ShowEmptyMessage() {
            System.Console.Write("Empty");
        }

        public void ShowMessage(string message) {
            System.Console.Write(message);
        }
       
        public string AplEcho(string text) {
            return text.ToUpper();
        }
    }
}
