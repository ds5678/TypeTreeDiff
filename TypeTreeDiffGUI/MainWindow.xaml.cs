using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TypeTreeDiff.Core.Diff;
using TypeTreeDiff.GUI.Comparer;

namespace TypeTreeDiff.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
		private string m_treeSortPropery;
		private ListSortDirection m_treeSortDirection;

		private string[] folderFilePaths;
		private int folderLeftSideFileIndex;

		public MainWindow()
		{
			InitializeComponent();

			LeftDump.DiffPosition = DumpControl.Position.Left;
			RightDump.DiffPosition = DumpControl.Position.Right;
			IndexIncreaseButton.Visibility = Visibility.Hidden;
			IndexDecreaseButton.Visibility = Visibility.Hidden;

			LeftDump.EventDumpDropped += OnDumpDropped;
			RightDump.EventDumpDropped += OnDumpDropped;
			LeftDump.EventFolderDropped += OnFolderDropped;
			RightDump.EventFolderDropped += OnFolderDropped;
			LeftDump.DropArea.EventFileDropped += DisableFolderDropMode;
			RightDump.DropArea.EventFileDropped += DisableFolderDropMode;
			LeftDump.EventDumpCreated += OnDumpCreated;
			RightDump.EventDumpCreated += OnDumpCreated;
			LeftDump.EventDumpSortOrderChanged += OnDumpSortOrderChanged;
			RightDump.EventDumpSortOrderChanged += OnDumpSortOrderChanged;
			LeftDump.EventDumpSelectionChanged += (index) => OnDumpSelectionChanged(RightDump, index);
			RightDump.EventDumpSelectionChanged += (index) => OnDumpSelectionChanged(LeftDump, index);
			LeftDump.EventDumpTypeTreesSelected += OnDumpTypeTreeSelected;
			RightDump.EventDumpTypeTreesSelected += OnDumpTypeTreeSelected;
			LeftDump.EventDumpHeaderSizeChanged += (offset) => OnDumpHeaderSizeChanged(RightDump, LeftDump);
			RightDump.EventDumpHeaderSizeChanged += (offset) => OnDumpHeaderSizeChanged(LeftDump, RightDump);
			LeftDump.EventDumpScrollChanged += (offset) => OnDumpScrollChanged(RightDump, offset);
			RightDump.EventDumpScrollChanged += (offset) => OnDumpScrollChanged(LeftDump, offset);

			LeftDump.EventTypeTreeBackClicked += OnTypeTreeBackClicked;
			RightDump.EventTypeTreeBackClicked += OnTypeTreeBackClicked;
			LeftDump.EventTypeTreeSelectionChanged += (index) => OnTypeTreeSelectionChanged(RightDump, index);
			RightDump.EventTypeTreeSelectionChanged += (index) => OnTypeTreeSelectionChanged(LeftDump, index);
			LeftDump.EventTypeTreeScrollChanged += (offset) => OnTypeTreeScrollChanged(RightDump, offset);
			RightDump.EventTypeTreeScrollChanged += (offset) => OnTypeTreeScrollChanged(LeftDump, offset);

			string[] args = Environment.GetCommandLineArgs();
			ProcessArguments(args);
		}

		private void ProcessArguments(string[] args)
		{
			if (args.Length < 2)
			{
				return;
			}

			string leftFile = args[1];
			if (!File.Exists(leftFile))
			{
				MessageBox.Show($"File '{leftFile}' doesn't exists");
				return;
			}

			LeftDump.ProcessDumpFile(leftFile);
			if (args.Length == 2)
			{
				return;
			}

			string rightFile = args[2];
			if (!File.Exists(rightFile))
			{
				MessageBox.Show($"File '{rightFile}' doesn't exists");
				return;
			}

			RightDump.ProcessDumpFile(rightFile);
		}

		// =================================
		// Custom events
		// =================================

		private void OnDumpDropped()
		{
			m_treeSortPropery = null;

			LeftDump.HideDragAndDrop();
			RightDump.HideDragAndDrop();
			LeftDump.ShowDumpView();
			RightDump.ShowDumpView();
		}

		private void OnFolderDropped(string folderPath)
		{
			IndexIncreaseButton.Visibility = Visibility.Visible;
			IndexDecreaseButton.Visibility = Visibility.Visible;
			folderLeftSideFileIndex = 0;
			folderFilePaths = Directory.GetFiles(folderPath, string.Empty, SearchOption.TopDirectoryOnly);
			Array.Sort(folderFilePaths, UnityVersionComparer.Instance);

			if (folderFilePaths.Length > 0)
			{
				LeftDump.ProcessDumpFile(folderFilePaths[0]);

				if (folderFilePaths.Length > 1)
				{
					RightDump.ProcessDumpFile(folderFilePaths[1]);
				}
			}
		}

		private void OnFolderIndexIncrease(object sender, RoutedEventArgs e)
		{
			if (folderLeftSideFileIndex == folderFilePaths.Length - 2)
			{
				return;
			}

			folderLeftSideFileIndex++;
			LeftDump.ProcessDumpFile(folderFilePaths[folderLeftSideFileIndex]);
			RightDump.ProcessDumpFile(folderFilePaths[folderLeftSideFileIndex + 1]);
		}

		private void OnFolderIndexDecrease(object sender, RoutedEventArgs e)
		{
			if (folderLeftSideFileIndex == 0)
			{
				return;
			}

			folderLeftSideFileIndex--;
			LeftDump.ProcessDumpFile(folderFilePaths[folderLeftSideFileIndex]);
			RightDump.ProcessDumpFile(folderFilePaths[folderLeftSideFileIndex + 1]);
		}

		private void DisableFolderDropMode(string _)
		{
			folderFilePaths = null;
			IndexIncreaseButton.Visibility = Visibility.Hidden;
			IndexDecreaseButton.Visibility = Visibility.Hidden;
		}

		private void OnDumpCreated()
		{
			if (LeftDump.Dump == null)
			{
				return;
			}
			if (RightDump.Dump == null)
			{
				return;
			}

			DBDiff diff = new DBDiff(LeftDump.DumpOptimized, RightDump.DumpOptimized);
			Dispatcher.InvokeAsync(() =>
			{
				LeftDump.FillLeftDump(diff);
				RightDump.FillRightDump(diff);
			});
		}

		private void OnDumpSortOrderChanged(string property)
		{
			if (property != m_treeSortPropery)
			{
				m_treeSortPropery = property;
				m_treeSortDirection = ListSortDirection.Ascending;
			}
			else
			{
				m_treeSortDirection = m_treeSortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
			}
			LeftDump.SortDumpItems(m_treeSortPropery, m_treeSortDirection);
			RightDump.SortDumpItems(m_treeSortPropery, m_treeSortDirection);
		}

		private void OnDumpSelectionChanged(DumpControl dump, int index)
		{
			dump.DumpListView.SelectedIndex = index;
		}

		private void OnDumpTypeTreeSelected(int classID)
		{
			LeftDump.ShowTypeTreeView(classID);
			RightDump.ShowTypeTreeView(classID);
			RightDump.TypeTreeListBox.Focus();
		}

		private void OnDumpHeaderSizeChanged(DumpControl dest, DumpControl source)
		{
			dest.DumpIDHeader.Width = source.DumpIDHeader.Width;
			dest.DumpNameHeader.Width = source.DumpNameHeader.Width;
		}

		private void OnDumpScrollChanged(DumpControl dump, double offset)
		{
			dump.SetDumpScrollPosition(offset);
		}

		private void OnTypeTreeBackClicked()
		{
			LeftDump.ShowDumpView();
			RightDump.ShowDumpView();
			ListViewItem listItem = (ListViewItem)RightDump.DumpListView.ItemContainerGenerator.ContainerFromIndex(RightDump.DumpListView.SelectedIndex);
			if (listItem != null)
			{
				listItem.Focus();
			}
		}

		private void OnTypeTreeSelectionChanged(DumpControl dump, int index)
		{
			dump.TypeTreeListBox.SelectedIndex = index;
		}

		private void OnTypeTreeScrollChanged(DumpControl dump, double offset)
		{
			dump.SetTypeTreeScrollPosition(offset);
		}

		// =================================
		// Form events
		// =================================

		private void OnDragEnter(object sender, DragEventArgs e)
		{
			if (sender == this)
			{
				LeftDump.ShowDragAndDrop();
				RightDump.ShowDragAndDrop();
			}
		}

		private void OnDragLeave(object sender, DragEventArgs e)
		{
			if (sender == this)
			{
				LeftDump.HideDragAndDrop();
				RightDump.HideDragAndDrop();
			}
		}
	}
}
