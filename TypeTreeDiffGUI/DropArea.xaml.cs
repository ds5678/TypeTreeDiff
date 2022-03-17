using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TypeTreeDiff.GUI
{
	public partial class DropArea : UserControl
	{
		public event Action<string> EventFileDropped;
		public event Action<string> EventFolderDropped;

		public static readonly DependencyProperty ActiveDropColorProperty =
			DependencyProperty.Register(nameof(ActiveDropColor), typeof(Color), typeof(DropArea));
		public static readonly DependencyProperty InactiveDropColorProperty =
			DependencyProperty.Register(nameof(InactiveDropColor), typeof(Color), typeof(DropArea), new PropertyMetadata(OnInactiveDropColorChanged));

		public Color ActiveDropColor
		{
			get { return (Color)GetValue(ActiveDropColorProperty); }
			set { SetValue(ActiveDropColorProperty, value); }
		}

		public Color InactiveDropColor
		{
			get { return (Color)GetValue(InactiveDropColorProperty); }
			set { SetValue(InactiveDropColorProperty, value); }
		}

		public DropArea()
		{
			InitializeComponent();
		}

		private static void OnInactiveDropColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DropArea dropArea = (DropArea)d;
			dropArea.Area.Background = new SolidColorBrush((Color)e.NewValue);
		}

		// =================================
		// Events
		// =================================

		private void OnDragEnter(object sender, DragEventArgs e)
		{
			Area.Background = new SolidColorBrush(ActiveDropColor);
		}

		private void OnDragLeave(object sender, DragEventArgs e)
		{
			Area.Background = new SolidColorBrush(InactiveDropColor);
		}

		private void OnDropped(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] filePaths = ((string[])e.Data.GetData(DataFormats.FileDrop));

				if (filePaths == null || filePaths.Length == 0)
				{
					throw new Exception("No files or folders were dropped.");
				}
				if (filePaths.Length > 1)
				{
					throw new Exception("Multiple files or folders were dropped but only one is supported.");
				}

				if (Directory.Exists(filePaths[0]))
				{
					EventFolderDropped?.Invoke(filePaths[0]);
				}
				else
				{
					EventFileDropped?.Invoke(filePaths[0]);
				}

			}
		}
	}
}
