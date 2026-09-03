local ui = require("ui")
local uiMenubar = require("ui.menubar")
local uiElements = require("ui.elements")
local uiUtils = require("ui.utils")

local widgetUtils = require("ui.widgets.utils")

local languageRegistry = require("language_registry")
local windowPersister = require("ui.window_position_persister")
local windowPersisterName = "bosuieditor"

local bosuieditorwin = {}
local bosuieditorwingrp = uiElements.group({})

function bosuieditorwin.getContent(element)
    local layout = uiElements.column({}):with(uiUtils.fillHeight(true))
    return layout
end

function bosuieditorwin.open(element)
	local lang = languageRegistry.getLanguage()
	local windowTitle = tostring(lang.ui.bosuieditor)
	local windowContent = bosuieditorwin.getContent(element)
	local window = uiElements.window(windowTitle,windowContent):with({height = WINDOW_STATIC_HEIGHT})
	local windowCloseCallback = windowPersister.getWindowCloseCallback(windowPersisterName)
	
	windowPersister.trackWindow(windowPersisterName, window)
	bosuieditorwingrp.parent:addChild(window)
    widgetUtils.addWindowCloseButton(window, windowCloseCallback)
    widgetUtils.preventOutOfBoundsMovement(window)
    widgetUtils.consumeKeyboardEvents(window)

	return window
end
function bosuieditorwin.getWindow()
--	bosuieditor.window = bosuieditorwin
	uiMenubar.menubar[#uiMenubar.menubar] = {"bosuieditor",{"open", bosuieditorwin.open}}
	return bosuieditorwingrp
end

return bosuieditorwin