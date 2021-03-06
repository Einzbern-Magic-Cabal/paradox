// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;
using SiliconStudio.Presentation.Services;

namespace SiliconStudio.Presentation.Dialogs
{
    public class FileOpenModalDialog : ModalDialogBase, IFileOpenModalDialog
    {
        internal FileOpenModalDialog(Dispatcher dispatcher, Window parentWindow)
            : base(dispatcher, parentWindow)
        {
            Dialog = new CommonOpenFileDialog { EnsureFileExists = true };
            Filters = new List<FileDialogFilter>();
            FilePaths = new List<string>();
        }

        /// <inheritdoc/>
        public bool AllowMultiSelection { get { return OpenDlg.Multiselect; } set { OpenDlg.Multiselect = value; } }

        /// <inheritdoc/>
        public IList<FileDialogFilter> Filters { get; set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<string> FilePaths { get; private set; }

        /// <inheritdoc/>
        public string InitialDirectory { get { return OpenDlg.InitialDirectory; } set { OpenDlg.InitialDirectory = value != null ? value.Replace('/', '\\') : null; } }

        /// <inheritdoc/>
        public string DefaultFileName { get { return OpenDlg.DefaultFileName; } set { OpenDlg.DefaultFileName = value; } }

        private CommonOpenFileDialog OpenDlg { get { return (CommonOpenFileDialog)Dialog; } }

        /// <inheritdoc/>
        public override DialogResult Show()
        {
            OpenDlg.Filters.Clear();
            foreach (var filter in Filters)
            {
                OpenDlg.Filters.Add(new CommonFileDialogFilter(filter.Description, filter.ExtensionList));
            }
            var result = InvokeDialog();
            FilePaths = result != DialogResult.Cancel ? new List<string>(OpenDlg.FileNames) : new List<string>();
            return result;
        }
    }
}