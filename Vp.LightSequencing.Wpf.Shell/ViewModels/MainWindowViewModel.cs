using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Vp.LightSequencing.Wpf.Shell.Model;

namespace Vp.LightSequencing.Wpf.Shell.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private const string TITLE = "VP Light Sequencing";

        #region Commands
        public DelegateCommand ClearShowCommand { get; private set; }
        public DelegateCommand LoadShowCommand { get; private set; }
        public DelegateCommand SaveShowCommand { get; private set; }        
        public DelegateCommand ShowGuideCommand { get; private set; }
        #endregion

        #region Properties
        /// <summary>
        /// Name of the object in Visual Pinball
        /// </summary>
        public string LightSeqName { get; set; } = "LightSeq001";
        public string LampshowInformation { get; set; }
        public string Script { get; set; }
        public string Title { get; set; } = TITLE;
        #endregion

        [PropertyChanged.DoNotNotify]
        public ObservableCollection<LightSequenceViewModel> LightSequenceViewModels { get; set; } =
            new ObservableCollection<LightSequenceViewModel>();

        #region Constructors

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            LightSequenceViewModels.CollectionChanged += LightSequenceViewModels_CollectionChanged;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<ListUpdatedEvent>().Subscribe(() => { UpdateScript(); });

            LoadShowCommand = new DelegateCommand(OnLoadShow, () => true);
            SaveShowCommand = new DelegateCommand(OnSaveShow, CanSaveShow);
            ClearShowCommand = new DelegateCommand(() => { LightSequenceViewModels?.Clear(); Title = TITLE; }, CanSaveShow);
            ShowGuideCommand = new DelegateCommand(() => 
            {
                try
                {
                    var guideHtmFile = Path.Combine(Directory.GetCurrentDirectory(), "Light Sequencer.htm");
                    var p = new Process() { StartInfo = new ProcessStartInfo() { UseShellExecute = true, FileName = guideHtmFile } };
                    p.Start();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }, () => true);
        }
        #endregion

        #region Support Methods
        private bool CanSaveShow() => LightSequenceViewModels.Count > 0;

        private void OnSaveShow()
        {
            var dialog = new SaveFileDialog()
            {
                InitialDirectory = GetOrCreateShowDirectory(),
                Filter = "Show Files(*.show)|*.show|All(*.*)|*"
            };
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, System.Text.Json.JsonSerializer.Serialize(this));
            }
        }

        private void OnLoadShow()
        {
            try
            {
                var dialog = new OpenFileDialog()
                {
                    InitialDirectory = GetOrCreateShowDirectory(),
                    Filter = "Show Files(*.show)|*.show|All(*.*)|*"
                };
                if (dialog.ShowDialog() == true)
                {
                    using (var fs = dialog.OpenFile())
                    using (var sr = new StreamReader(fs))
                    {
                        var vm = System.Text.Json.JsonSerializer.Deserialize<MainWindowViewModel>(sr.ReadToEnd());
                        if (vm != null)
                        {
                            if (LightSequenceViewModels == null)
                            {

                            }
                            else
                            {
                                this.LightSequenceViewModels.Clear();
                            }

                            foreach (var item in vm.LightSequenceViewModels)
                            {
                                LightSequenceViewModels.Add(item);
                            }

                            Title = $"{TITLE} - Loaded Show: {Path.GetFileName(dialog.FileName)}";
                            LightSeqName = vm.LightSeqName;
                            Script = vm.Script;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("An error occured loading the show. " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the local shows directory for loading / saving
        /// </summary>
        /// <returns></returns>
        private string GetOrCreateShowDirectory()
        {
            var currDir = Directory.GetCurrentDirectory();
            var showDir = Path.Combine(currDir, "shows");
            if (!Directory.Exists(showDir)) Directory.CreateDirectory(showDir);

            return showDir;
        }

        private void LightSequenceViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems?.Count > 0)
            {
                var s = e.NewItems[0];
                var seqVm = s as LightSequenceViewModel;
                seqVm.PropertyChanged += SeqVmPropertyChanged;
            }
            else if (e.OldItems != null)
            {
                foreach (var item in e?.OldItems)
                {
                    var seqVm = item as LightSequenceViewModel;
                    seqVm.PropertyChanged -= SeqVmPropertyChanged;
                }
            }

            ClearShowCommand.RaiseCanExecuteChanged();
            SaveShowCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Updates the script when grid changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeqVmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Name")
            {
                var seqVm = sender as LightSequenceViewModel;
                if(seqVm != null)
                {
                    //todo: user changed the name so update the tool tips?
                    var seqName = seqVm.Name.ToString();
                    var tipName = $"{seqName.Replace("On","").Replace("Off","")}Tip";
                    if (Application.Current.Resources.Contains(tipName))
                    {
                        seqVm.Tip = Application.Current.Resources[tipName].ToString();
                    }
                    tipName = $"{seqName}Tail";
                    if (Application.Current.Resources.Contains(tipName))
                    {
                        seqVm.TailTip = Application.Current.Resources[tipName].ToString();
                    }
                    else
                    {
                        if ((int)seqVm.Name > 48)
                            seqVm.TailTip = "This is specified in degrees. Ie. A value of 90 will follow 90 degrees behind the head effect. 0 (or not specified) means no 'Tail'";
                        else
                            seqVm.TailTip = "Normal Use";
                    }
                    tipName = $"{seqName}Pause";
                    if (Application.Current.Resources.Contains(tipName))
                    {
                        seqVm.PauseTip = Application.Current.Resources[tipName].ToString();
                    }
                    else seqVm.PauseTip = "Normal Use";
                    tipName = $"{seqName}Repeat";
                    if (Application.Current.Resources.Contains(tipName))
                    {
                        seqVm.RepeatTip = Application.Current.Resources[tipName].ToString();
                    }
                    else seqVm.RepeatTip = "Normal Use";
                }                
            }
            UpdateScript();
        }

        /// <summary>
        /// Updates the script text block
        /// </summary>
        internal void UpdateScript()
        {
            LampshowInformation = $"Total length: {LightSequenceViewModels.Sum(x => x.Length)}";
            Script = null;

            for (int i = 0; i < LightSequenceViewModels.Count; i++)
            {
                if(i != 0)
                {
                    //if the previous sequence interval the same as this one then skip adding another line of script
                    bool intervalMatched = LightSequenceViewModels[i-1].Interval == LightSequenceViewModels[i].Interval;
                    Script += LightSequenceViewModels[i].ToString(LightSeqName, !intervalMatched) + Environment.NewLine;
                }
                else //always add update interval to first line
                {
                    Script += LightSequenceViewModels[i].ToString(LightSeqName) + Environment.NewLine;
                }                
            }
        }         
        #endregion
    }
}