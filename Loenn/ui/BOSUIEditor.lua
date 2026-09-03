local bosuieditor = {}

bosuieditor.window = nil

function bosuieditor.open(element)
	if bosuieditor.window then
		bosuieditor.window.open(element)
	end
end

return bosuieditor