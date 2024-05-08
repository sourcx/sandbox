#!/usr/bin/python
# De programma opbouw:
# - 1. imports & requirements
# - 2. globale variabelen & constanten
# - 3. functie definities
# - 4. programma start

# 1a. imports
import gtk
import os
import pygtk
import string
import sys
import vacationgui
from socket import gethostname
from getpass import getuser

pygtk.require('2.0')

VACATION_GENERATED = "generated by Vacation"

aliasesFile = ".vacation.aliases"
mailFilter 	= ".mailfilter"
subjectFile = ".vacation.subj"
bodyFile    = ".vacation.body"
messageFile = ".vacation.msg"


class Vacation:
	"Main program"

	def bepaalFileLengte (self, file):
		file.seek (0, 2)
		fileLengte = file.tell()
		file.seek (0)	
		return fileLengte


	def fileBestaat (self, fileName):
		if os.path.exists (fileName):
			return True
		else:
			return False



	def printStatus (self, opVakantie):
		sys.stdout.write ("De vacation message staat ")
	
		if opVakantie: 
			print "AAN"
		else:
			print "UIT"


	def getFileText (self, fileName):
		if (self.fileBestaat(fileName) == False):
			return ""

		file = open (fileName, "r")
	
		fileLengte = self.bepaalFileLengte (file)	
		fileTekst =  file.read (fileLengte)

		file.close()
		return fileTekst


	def opVakantie (self, mailFilter):
		if self.fileBestaat (mailFilter):
			file 	   	= open (mailFilter, "rb")
			fileLengte 	= self.bepaalFileLengte (file)
			fileTekst  	= file.read (fileLengte)
			bevat 		= string.find (fileTekst, VACATION_GENERATED)

			if bevat >= 0:
				return True
			else:
				return False;
	

	def save (fromGui, opVakantie, subject, body, aliases):
		# Save subject
		file = open (subjectFile,"w")
		file.writelines (subject)
		file.close()

		# Save body
		file = open (bodyFile, "w")
		file.writelines (body)
		file.close()
		
		# Save aliases
		file = open (aliasesFile,"w")
		file.writelines (aliases)
		file.close()

		# Create Full Message 
		fullmessage = ""


		aliaseslist = aliases.split()
		aliasesall = ""

		for alias in aliaseslist:
			aliasesall += "-a " + alias + " "

		if (aliases != ""):
			fullmessage += "From: " + aliaseslist[0] + "\n"
		

		fullmessage += "Precedence: bulk\n"
		fullmessage += "X-Delivery-By: Vacation (OpenOffice.nl)\n"
		fullmessage += "Subject: " + subject + "\n\n"
		fullmessage += body

		
		# write message in file
		file = open (".vacation.msg", "w")
		file.writelines (fullmessage)
		file.close()


		# Create .mailfilter info
		mailfilterMessage = "cc \"|vacation " + getuser() + " " + aliasesall + " -f .vacation.db -m .vacation.msg; sleep 5; true This line was generated by Vacation (OpenOffice.nl)\""
	
	
		file = open (mailFilter, "w")
		
		# Write into .mailfilter if vacation is enabled
		if (opVakantie == True):
			file.writelines (mailfilterMessage)

		# Otherwise empty the .mailfilter
		else:
			file.write (" ")	

		file.close()


def main ():
	vac 		= Vacation()
	opVakantie	= vac.opVakantie  (mailFilter)
	subject 	= vac.getFileText (subjectFile)
	body 		= vac.getFileText (bodyFile)
	aliases		= vac.getFileText (aliasesFile)
	
	screen = vacationgui.MainScreen()
	
	screen.setOnVacation (opVakantie)
	screen.setSubject (subject)
	screen.setBody (body)
	screen.setAliases (aliases)
	
	screen.buildScreen()
	screen.main (vac)


if __name__ == "__main__":
	main()
