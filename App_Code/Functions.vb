Imports Microsoft.VisualBasic
Imports System.Data

Public Class Functions
    Dim tempPage As String
    Dim List_Proposition() As String

    Public Function Parsing(ByVal SearchInput As String, ByVal PropositionList() As String)
        Dim isStartwithAND As Boolean = False
        Dim List2(1000) As String
        Dim ORList() As String
        Dim Len_ORList As Integer = 0
        Dim Len_List As Integer = 0
        Try
            ORList = SearchInput.Split(New Char() {""""}, StringSplitOptions.RemoveEmptyEntries)
            'List_Proposition = GetPropositionsList()
            List_Proposition = PropositionList

            If ORList.Length = 0 Then
                Return ORList
            End If

            If SearchInput(0) = """" Then
                isStartwithAND = True
            End If

            For Len_ORList = 0 To ORList.Length - 1
                ORList(Len_ORList) = Trim(ORList(Len_ORList))
                If isStartwithAND Then
                    If (Len_ORList Mod 2 = 0) Then
                        If Not_Exists_Keyword(ORList(Len_ORList)) Then
                            List2(Len_List) = ORList(Len_ORList)
                            Len_List = Len_List + 1
                        End If
                    Else
                        Dim ORList1() As String = ORList(Len_ORList).Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries)
                        For Each parse1 As String In ORList1
                            parse1 = Trim(parse1)
                            If parse1 <> "" AndAlso Not_Exists_Keyword(parse1) Then
                                List2(Len_List) = parse1
                                Len_List = Len_List + 1
                            End If
                        Next
                    End If
                Else
                    If (Len_ORList Mod 2 <> 0) Then
                        If Not_Exists_Keyword(ORList(Len_ORList)) Then
                            List2(Len_List) = ORList(Len_ORList)
                            Len_List = Len_List + 1
                        End If
                    Else
                        Dim ORList1() As String = ORList(Len_ORList).Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries)
                        For Each parse1 As String In ORList1
                            parse1 = Trim(parse1)
                            If parse1 <> "" AndAlso Not_Exists_Keyword(parse1) Then
                                List2(Len_List) = parse1
                                Len_List = Len_List + 1
                            End If
                        Next
                    End If
                End If
            Next

            Array.Resize(List2, Len_List)

        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        Finally
        End Try
        Return List2

    End Function

    Public Function GetPropositionsList()
        Dim connForImport As OleDb.OleDbConnection
        Dim FIleNameandPath As String = ""
        Dim connectionString As String = ""
        Dim commandImportString As String = ""
        Dim commandValidateString As String = ""
        Dim commandForImport As OleDb.OleDbCommand
        Dim fileStreamObjToRead As System.IO.FileStream
        Dim SheetName As String
        Dim TopStr As String
        Dim NoofLines As Integer
        Dim Lists As String = ""
        Dim ExportDataset As DataSet
        Dim PropositionsList As New ArrayList
        Try
            SheetName = "Sheet1"
            FIleNameandPath = HttpRuntime.BinDirectory.Replace("\bin", "\Files")
            FIleNameandPath = FIleNameandPath & "Propositions.xls"

            If System.IO.File.Exists(FIleNameandPath) Then
                Try
                    fileStreamObjToRead = System.IO.File.OpenRead(FIleNameandPath)
                Catch ex As IO.IOException
                    fileStreamObjToRead = Nothing
                    PropositionsList.Add("a")
                    Return PropositionsList.ToArray("System.String".GetType)
                    Exit Function
                End Try

                If fileStreamObjToRead.CanRead() Then
                    fileStreamObjToRead.Close()
                    fileStreamObjToRead = Nothing
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FIleNameandPath + ";Extended Properties='Excel 8.0;'"
                    commandImportString = "[" + SheetName + "$]"
                    commandValidateString = "select Top 1 * from " + commandImportString

                    Try
                        NoofLines = getNoofLinesinExcel(FIleNameandPath, SheetName)
                        TopStr = "TOP " & CStr(NoofLines) & " "
                    Catch ex As Exception
                        MsgBox("File In Use, Try Again", MsgBoxStyle.Critical, "Error")
                        PropositionsList.Add("a")
                        Return PropositionsList.ToArray("System.String".GetType)
                        Exit Function
                    End Try

                    connForImport = New System.Data.OleDb.OleDbConnection(connectionString)
                    connForImport.Open()
                    commandForImport = connForImport.CreateCommand()
                    Try
                        PropositionsList.Clear()
                        If validateFiletoImport(connForImport, commandForImport, commandValidateString) Then
                            ExportDataset = readFiletoDummyDataSet(connForImport, commandForImport, commandImportString, TopStr)
                            For Each dataRow As DataRow In ExportDataset.Tables(0).Select()
                                PropositionsList.Add(dataRow.Item(0))
                                Lists = Lists & "," & dataRow.Item(0)
                            Next
                        End If
                    Catch ex As Exception
                        MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
                    End Try
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
        Return PropositionsList.ToArray("System.String".GetType)
    End Function

    Private Function getNoofLinesinExcel(ByVal FileNameandPath As String, ByVal sheetName As String) As Long
        Dim Exapp As New Excel.Application
        Dim ExWrkBook As Excel.Workbook
        Dim LinesinExcel As Long
        Try
            Exapp = New Excel.ApplicationClass
            Try
                ExWrkBook = Exapp.Workbooks.Open(FileNameandPath)

                LinesinExcel = (ExWrkBook.Worksheets.Item(sheetName).Range("A1").End(-4121).Row) - 1

                ExWrkBook.Close()
                ExWrkBook = Nothing
                Exapp.Quit()
                Exapp = Nothing

                GC.Collect()
                Return LinesinExcel

            Catch ex As Exception
                Throw ex
            End Try

            Return 0
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function validateFiletoImport(ByVal connforFile As OleDb.OleDbConnection, ByVal commandforFile As OleDb.OleDbCommand, ByVal CommandValidateString As String)
        Dim allDataSetFieldsString As String = ""
        Dim allDataSetFieldsLengthString As String = ""
        Dim allExcelFieldsString As String = ""
        Dim dataReader As OleDb.OleDbDataReader
        Dim fieldRowIndex As Integer
        Dim Splitter As String = "@#"

        Try
            allDataSetFieldsString = "Marker"
            allDataSetFieldsLengthString = 5

            commandforFile.CommandText = CommandValidateString
            Try
                dataReader = commandforFile.ExecuteReader
            Catch ex As OleDb.OleDbException
                dataReader = Nothing
                Return False
            End Try

            Try
                If dataReader.HasRows Then
                    For fieldRowIndex = 0 To dataReader.FieldCount - 1
                        allExcelFieldsString = allExcelFieldsString + Splitter + dataReader.GetName(fieldRowIndex).Trim().ToUpper()
                    Next
                    allExcelFieldsString = allExcelFieldsString + Splitter
                    dataReader.Close()
                End If

                If dataReader.IsClosed = False Then
                    dataReader.Close()
                End If

            Catch ex As Exception
                MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
                Return False
            End Try
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
        Return True
    End Function

    Private Function readFiletoDummyDataSet(ByVal connforFile As OleDb.OleDbConnection, ByVal commandforFile As OleDb.OleDbCommand, ByVal CommandValidateString As String, ByVal TopStr As String) As DataSet
        Dim datasetFillAdapter As New OleDb.OleDbDataAdapter
        Dim getDataSet As New DataSet

        Try
            Dim CommString As String
            Dim EXLSelectString As String = ""

            CommString = "Select " + TopStr + " * from " + CommandValidateString

            commandforFile.CommandText = CommString

            datasetFillAdapter.SelectCommand = commandforFile

            datasetFillAdapter.Fill(getDataSet, "Table1")
            connforFile.Close()

        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
        Return getDataSet
    End Function

    Private Function Not_Exists_Keyword(ByVal keyword As String)
        Try
            For count As Integer = 0 To List_Proposition.Length - 1
                If List_Proposition(count) = Trim(keyword).ToLower() Then
                    Return False
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
        Return True
    End Function

    Public Function Output(ByVal BTree As Node, ByVal SearchString() As String)
        Dim OutputString As New ArrayList
        Dim AndList() As String
        Dim isFirst As Boolean = False
        Dim ORArray() As String
        Dim Searcher_obj As New Searcher
        Dim tempBTree As New Node
        Try
            OutputString.Clear()
            For ORCount As Integer = 0 To SearchString.Length - 1
                Dim AndString() As String
                AndString = Nothing
                AndList = SearchString(ORCount).Split("+")
                isFirst = False
                For AndCount As Integer = 0 To AndList.Length - 1
                    If Trim(AndList(AndCount)) <> "+" Then
                        Dim Pages() As String
                        AndList(AndCount) = Trim(AndList(AndCount))
                        tempBTree = BTree
                        If isFirst = False Then
                            AndString = Searcher_obj.Search(tempBTree, AndList(AndCount))
                            isFirst = True
                        ElseIf Not AndString Is Nothing AndAlso AndString.Length > 0 Then
                            Pages = Searcher_obj.Search(tempBTree, AndList(AndCount))
                            If Not Pages Is Nothing Then
                                AndString = RemovePages(Pages, AndString)
                            End If
                        End If
                    End If
                Next
                ORArray = OutputString.ToArray("System.String".GetType)
                If Not AndString Is Nothing Then
                    For count As Integer = 0 To AndString.Length - 1
                        tempPage = AndString(count)
                        If Not Array.Exists(ORArray, AddressOf ExistPages) Then
                            OutputString.Add(AndString(count))
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try

        Return OutputString.ToArray("System.String".GetType)
    End Function

    Public Function RemovePages(ByVal PageList() As String, ByVal AndString() As String)
        Dim tString As New ArrayList
        Try
            If PageList.Length = 0 Then
                Return PageList
            End If

            For count As Integer = 0 To PageList.Length - 1
                tempPage = PageList(count)
                If Array.Exists(AndString, AddressOf ExistPages) Then
                    tString.Add(PageList(count))
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try

        Return tString.ToArray("System.String".GetType)
    End Function

    Public Function ExistPages(ByVal s As String) As Boolean
        Try
            If s = tempPage Then
                Return True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
        Return False
    End Function

    Public Function ListofPageNos(ByVal OutputAllPages() As String)
        Dim PageLists As New ArrayList
        Dim Countpages As Integer
        Dim PagesLength As Decimal
        Try
            PagesLength = OutputAllPages.Length / 10
            If PagesLength > Math.Truncate(PagesLength) Then
                Countpages = Math.Truncate(PagesLength) + 1
            Else
                Countpages = PagesLength
            End If
            For Count As Integer = 1 To Countpages
                PageLists.Add(Count)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
        Return PageLists
    End Function

End Class
