tool
extends EditorPlugin

var cpm

func _enter_tree():
	cpm = preload("res://addons/color-template-editor/ColorTemplateManager.tscn").instance()
#	cpm.undoredo = get_undo_redo()
	add_control_to_bottom_panel(cpm, "Simple Color Template Editor")

func _exit_tree():
	remove_control_from_bottom_panel(cpm)
#	cpm.free()
