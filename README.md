> :warning: **If you are a current student at the University of Utah**:
> 
> Plagiarism is against the School of Computing's code of conduct.
> All work done here was from scratch and/or skeleton code provided by the class.
>
> You will learn nothing if you cheat. You are cheating on yourself.
> This project made me into a great programmer. Do the same for yourself.
>
> The project is only to showcase my past work and show competency in this subject
>
> Again, you will get caught as Gradescope will pick this up.
>
> Will only be up during times I want to showcase my work.


# Spreadsheet_Project

# ps6-los_coders
ps6-los_coders created by GitHub Classroom

Authors: Kevin Pineda & Cristian Tapiero

Design Decisions:
When we started designing the spreadsheet GUI we decided to include two text boxes
one of them contains the value of the cell and it cannot be edited and the other is
for the contents of the cell which can be edited to allow the user to set the contents.

Later we discussed how to implement the save and open functions on the menu and the way 
they work as they prompt the user to save if a change has been made to the spreadsheet
and if you try opening a new spreadsheet it will ask to save it before it clears the form.

We decided to clear the spreadsheet form instead of closing the current one and opening a new window 
when opening a new file because we consider the resources to open new spreadsheets in a different window 
every time can be costly over time and we wanted also to keep our program simple. Despite of this, the
user is able to open a new spreadsheet and open a file if what he wants is working in multiple at the same time.

External code resources:
Used example code in SaveFileDialog Class at docs.microsoft.com
Used example code in OpenFileDialog Class at docs.microsoft.com
www.daveoncsharp.com to learn how to make the close button X hold while the user is prompted to save the spreadsheet.

Summary of Additional Features:
For the extra features, we changed the colors of the spreadsheet to mimic a dark theme and
ask the user if closing a spreadsheet that has been modified using the X button on the top right of the window.

Special Instructions for Use:
When first opening the spreadsheet the user has to manually go to the content text box to type the content of the cell
after doing so the content text box automatically is focused when the user clicks on any cell.

Entry Dates:
Commits on Oct 19, 2021
Commits on Oct 20, 2021
Commits on Oct 21, 2021
Commits on Oct 22, 2021


![alt text](https://github.com/pinedakevin/Spreadsheet_Project/blob/Spreadsheet/Spreadsheet_Screeshot.png?raw=true)
