using Prism.Mvvm;
using System.ComponentModel;
using Vp.LightSequencing.Wpf.Shell.Model;

namespace Vp.LightSequencing.Wpf.Shell.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class LightSequenceViewModel : BindableBase
    {
        public Sequence Name { get; set; }
        public int Interval { get; set; } = 10;
        public string IntervalTip { get; set; } = @"
The speed that the Light Sequencer Runs at. It is best to define this in the script as it is definable per effect. 
If you want all effects to run at a set speed then this is the place to do it.";
        public int Tail { get; set; }
        public int Repeat { get; set; } = 1;
        public int Pause { get; set; }
        public int Length { get; set; }
        public string Tip { get; set; }
        public string TailTip { get; set; }
        public string PauseTip { get; set; }
        public string RepeatTip { get; set; }

        public LightSequenceViewModel()
        {

        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            Length = SequenceHelper.GetSequenceLength(Name, Interval, Tail, Repeat, Pause);
            base.OnPropertyChanged(args);
        }

        /// <summary>
        /// Returns update interval and command
        /// </summary>
        /// <returns></returns>
        public string ToString(string lightSeqName = "LightSeqAttract", bool addUpdateInterval = true)
        {
            string cmd = string.Empty;
            if(addUpdateInterval) cmd = $"{lightSeqName}.UpdateInterval = {Interval}\n";
            cmd += $"{lightSeqName}.Play {Name},{Tail},{Repeat},{Pause}' total ms: {Length}";
            return cmd;
        }
    }
}
