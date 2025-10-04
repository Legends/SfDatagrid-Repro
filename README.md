# SfDatagrid-Repro

SfDataGrid:

	EditTrigger="OnDoubleTap"

## Aim
The error tooltip should open automatically when the user edits a cell and confirms using Enter key.

## Issue/Error

I get a 

	Cannot animate the 'IsOpen' property on a'System.Windows.Controls.ToolTip' 
	using a 'System.Windows.Media.Animation.ObjectAnimationUsingKeyFrames

with InnerException:

	InvalidOperationException: The animation(s) applied to the 'IsOpen' property calculate a current value of 'True', 
	which is not a valid value for the property.

## Reproduction

Double tap a cell to enter edit mode and press Enter to confirm the edit.
The cell validation is hard-coded to always fail, so the error tooltip should open automatically.

