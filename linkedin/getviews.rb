require 'watir-webdriver'
require 'watir-scroll'

USERNAME = ""
PASSWORD = ""

browser = Watir::Browser.new :chrome
browser.goto "http://linkedin.com"

browser.element(id: 'login-email').click
browser.text_field(id: 'login-email').set(USERNAME)
browser.element(id: 'login-password').click
browser.text_field(id: 'login-password').set(PASSWORD)
browser.button(name: 'submit').click
browser.li(class: "nav-item activity-tab", index: 2).hover
browser.span(class: "connection-tab-header-action").when_present.click

browser.scroll.to :bottom

(0..11).each do |n|
  link = browser.link(class: "name", index: n)
  link.when_present.click(:command)
end
#
# browser.scroll.to :bottom
#
# (12..20).each do |n|
#   link = browser.link(class: "name", index: n)
#   link.when_present.click(:command)
# end
#
# browser.scroll.to :bottom
