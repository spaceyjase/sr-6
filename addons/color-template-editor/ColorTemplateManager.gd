tool
extends Control

onready var to_picker_button   : Button   = $VBoxContainer/HBoxContainer/ToPickerButton
onready var from_picker_button : Button   = $VBoxContainer/HBoxContainer/FromPickerButton
onready var color_list         : TextEdit = $VBoxContainer/ColorList

var colors : Array

func _to_picker():
	var arr: PoolColorArray
	
	for c in colors:
		arr.append(c)
	
	var ep = EditorPlugin.new()
	ep.get_editor_interface() \
		.get_editor_settings() \
		.set_project_metadata("color_picker", "presets", arr)
	ep.free()

func _alert(title: String, text: String):
	var dialog = AcceptDialog.new()
	dialog.window_title = title
	dialog.dialog_text = text
	dialog.connect('modal_closed', dialog, 'queue_free')
	add_child(dialog)
	dialog.popup_centered()

func _from_picker():
	var ep = EditorPlugin.new()
	var from_picker_colors : PoolColorArray = ep.get_editor_interface() \
		.get_editor_settings() \
		.get_project_metadata("color_picker", "presets")
	colors = []
	for c in from_picker_colors:
		colors.append(c)
	ep.free()

func _parse_colors():
	colors = []
	var colors_str = color_list.text
	colors_str = colors_str.replace(",", " ")
	colors_str = colors_str.replace("\t", " ")
	colors_str = colors_str.replace(" ", "\n")
	for color_str in colors_str.split("\n"):
		if color_str.length() > 3:
			colors.append(Color(color_str))

func _update_text():
	var text = ""
	for color in colors:
		var c : Color = color
		text += c.to_html(true) + "\n"
	color_list.text = text

func _on_ToPickerButton_pressed():
	_parse_colors()
	if colors.size() > 0:
		_to_picker()
		_alert("Updated", "The color list has been updated.")
	else:
		_alert("No data", "There are no colors to set.")

func _on_FromPickerButton_pressed():
	_from_picker()
	if colors.size() > 0:
		_update_text()
		_alert("Updated", "The color list has been loaded.")
	else:
		_alert("No data", "The picker colors is empty.")


func _ready():
	to_picker_button.connect("pressed", self, "_on_ToPickerButton_pressed")
	from_picker_button.connect("pressed", self, "_on_FromPickerButton_pressed")
