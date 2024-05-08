import pygtk, gtk, os, vacation

# Global variables


class MainScreen:
	"Main GUI for vacation"

	vac             	= vacation.Vacation()
	onVacation 			= False
	subjectText     	= ""
	subjectTextfield 	= gtk.Entry()
	bodyBuffer			= gtk.TextBuffer()
	aliasesBuffer 		= gtk.TextBuffer()

	vinkje 				= gtk.Image()
	vinkje.set_from_file ("./vinkje.gif")
	
	
	def makeButton (self, text):
		return gtk.Button (text)


	def makeEntry (self, text):
		entry = gtk.Entry ()
		entry.set_text (text)
		return entry
		

	def makeLabel (self, text):
		return gtk.Label (text)
	
		
	def makeTextView (self, textBuffer):
		textview = gtk.TextView (textBuffer)
		textview.set_editable (True)
		textview.set_wrap_mode (gtk.WRAP_WORD)
		textview.set_border_width (2)
		textview.grab_focus()
		return textview

	def makeScrolledWindow (self, textview, width, height):
		vadjust			= gtk.Adjustment (30, 1000, 100, 2, 10, (height-2))
		
		scrolledWindow 	= gtk.ScrolledWindow (None, vadjust)
		scrolledWindow.set_size_request (width, height)
		scrolledWindow.add_with_viewport (textview)
		scrolledWindow.set_policy(gtk.POLICY_AUTOMATIC, gtk.POLICY_ALWAYS)

		#misschien http://www.async.com.br/faq/pygtk/index.py?req=edit&file=faq10.010.htp

		return scrolledWindow


	def makeHBox (self, one, another, space):
		box = gtk.HBox (False, space)

		box.pack_start (one, False, False, 5)
		one.show()

		box.pack_start (another, False, False, 5)
		another.show()

		return box


	def makeVBox (self, one, another, space):
		box = gtk.VBox (False, space)

		box.pack_start (one, False, False, 5)
		one.show()

		box.pack_start (another, False, False, 5)
		another.show()

		return box


	def makeWindow (self, box):
		window = gtk.Window()
		window.set_default_size (800, 600)
		window.add (box)
		window.connect ("delete-event", lambda a,b: gtk.main_quit())

		return window


	def menuitem_response (self):
		sys.stdout.write ("De vacation message staat ... bla")


	def align (self, widget, type):
		align = gtk.Alignment()
		if (type == "left"):
			align.set (0, 0, 0, 0)
		elif (type == "right"):
			align.set (1, 0, 0, 0)
		else:
			return widget
	
		align.add (widget)

		return align


	def saveButtoncallback (self, widget, data=None):
		start	 	= self.bodyBuffer.get_start_iter()
		end		 	= self.bodyBuffer.get_end_iter()
		bodytext 	= self.bodyBuffer.get_text (start, end)
		
		start 		= self.aliasesBuffer.get_start_iter()
		end		 	= self.aliasesBuffer.get_end_iter()
		aliasestext = self.aliasesBuffer.get_text (start, end)

		self.vac.save (self.onVacation, self.subjectTextfield.get_text(), bodytext, aliasestext)


	def enableVacation (self, widget):
		self.onVacation = True
		self.vinkje.set_from_file ("./vinkje.gif")
		widget.set_image (self.vinkje)
	
	
	def disableVacation (self, widget):
		self.onVacation = False
		self.vinkje.set_from_file ("./vinkjerood.gif")
		widget.set_image (self.vinkje)



	def buildScreen (self):
	# LEVEL 6
		# widgets not containing boxes:
		onButton			= self.makeButton ("Enable")
		onButton.connect ("clicked", self.enableVacation)
		onButton.set_size_request (100,50)

		offButton			= self.makeButton ("Disable")
		offButton.connect ("clicked", self.disableVacation)
		offButton.set_size_request (100,50)
		
		if (self.onVacation == True):
			self.vinkje.set_from_file ("./vinkje.gif")
			onButton.set_image (self.vinkje)
		else:
			self.vinkje.set_from_file ("./vinkjerood.gif")
			offButton.set_image (self.vinkje)
		

	# LEVEL 5
		# contains on and off button box
		hboxOnOff			= self.makeHBox (onButton, offButton, 10)

		# widgets not containing boxes:
		onVacationTitle		= self.makeLabel ("Status")
		
	# LEVEL 4
		# contains onVacationTitle and onOff-box
		vboxOnVacation		= self.makeVBox (onVacationTitle, hboxOnOff, 5)
		saveButton			= self.makeButton ("Save")		
		saveButton.connect ("clicked", self.saveButtoncallback)

		# widgets not containing boxes:
		addressTitle		= self.makeLabel ("Email aliases (seperate with "
										 "whitespace)")
		addressTextfield 	= self.makeTextView (self.aliasesBuffer)

		scrollAddresses 	= self.makeScrolledWindow (addressTextfield, 300,150)
	

		messageTitle		= self.makeLabel (" Body:")
		messageTitle		= self.align (messageTitle, "left")
		messageTextfield	= self.makeTextView (self.bodyBuffer)
		scrollMessage 	    = self.makeScrolledWindow (messageTextfield, 640,300)
		
		subjectTitle		= self.makeLabel (" Subject");
		subjectTitle 		= self.align (subjectTitle, "left")

		self.subjectTextfield	= self.makeEntry (self.subjectText);
		
	# LEVEL 3
		# contains onVacation and subject box
		vboxLeftTop			= self.makeVBox (vboxOnVacation, saveButton, 10)

		# contains addressTitle and addressTextfield
		vboxAdresses		= self.makeVBox (addressTitle, scrollAddresses, 0)

		# contains subjectTitle and subjectField
		vboxSubject			= self.makeVBox (subjectTitle, self.subjectTextfield, 0) 

		# contains messageTitle and addressTextField
		vboxMessage			= self.makeVBox (messageTitle, scrollMessage, 0)

	# LEVEL 2
		# contains on/off, subject and addresses box
		hboxUp			= self.makeHBox (vboxLeftTop, vboxAdresses, 10)

		# contains messageTitle and messageTextfield
		vboxDown		= self.makeVBox (vboxSubject, vboxMessage, 10)

	# LEVEL 1
		# toplevel vbox
		vboxAll			= self.makeVBox (hboxUp, vboxDown, 10)

		# main window
		window			= self.makeWindow (vboxAll)

		window.show_all()
		window.show()


	def setOnVacation (self, opVakantie):
		self.onVacation = opVakantie

	def setSubject (self, subject):
		self.subjectText = subject

	def setBody (self, body):
		self.bodyBuffer.set_text (body)

	def setAliases (self, aliases):
		self.aliasesBuffer.set_text (aliases)

	def main (self, vac):
		self.vac = vac
		gtk.main()
