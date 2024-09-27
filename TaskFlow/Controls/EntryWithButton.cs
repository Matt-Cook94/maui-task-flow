using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Controls
{
    public class EntryWithButton : HorizontalStackLayout
    {
        public EntryWithButton()
        {
            var entry = new Entry();
            entry.BackgroundColor = Colors.White;
            Children.Add(entry);

            var button = new Button { Text = "Test" };
            button.CornerRadius = 0;
            Children.Add(button);
        }
    }
}
