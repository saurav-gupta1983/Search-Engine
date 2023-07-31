Imports Microsoft.VisualBasic
Imports System.Data
Imports System.IO

Public Class Searcher
    Dim Index As Node

    Public Function Search(ByVal BTree As Node, ByVal keyword As String)
        Dim TreeStructure As Node
        Dim Result As Integer
        Dim Pages() As String

        TreeStructure = BTree
        Pages = Nothing
        Try
            While Not (TreeStructure Is Nothing)
                For Count As Integer = 0 To TreeStructure.countNodes - 1
                    Result = StrComp(keyword.ToLower(), TreeStructure.Value(Count).ToLower(), 0)
                    If Result = 0 Then
                        Pages = TreeStructure.Pages(Count)
                        Exit While
                    End If
                    If Result < 0 Then
                        TreeStructure = TreeStructure.ChildNodes(Count)
                        Exit For
                    End If
                    If Result > 0 AndAlso Count = TreeStructure.countNodes - 1 Then
                        TreeStructure = TreeStructure.ChildNodes(Count + 1)
                        Exit For
                    End If
                Next
            End While
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
        Return Pages
    End Function

    Public Function GetFileList()
        Dim fileName As String
        Dim filePath As String
        Dim directoryPath As String

        Try
            directoryPath = HttpRuntime.BinDirectory.Replace("\bin", "\Files")
            fileName = "out.txt"

            If Not (System.IO.Directory.Exists(directoryPath)) Then
                System.IO.Directory.CreateDirectory(directoryPath)
            End If

            filePath = directoryPath & fileName

            If Not (System.IO.File.Exists(filePath)) Then
                Return Index
                Exit Function
            End If

            Dim levels_FS As FileStream
            Dim levels As String
            Dim levelNumber As Integer = 0

            levels_FS = System.IO.File.OpenRead(filePath)
            Dim level_S As New StreamReader(levels_FS)
            While 1
                levels = level_S.ReadLine()
                If levels Is Nothing Then
                    Exit While
                End If
                levels = levels.Split("\")(levels.Split("\").Length - 1)
                construct_Tree(directoryPath & levels, levelNumber)
                levelNumber = levelNumber + 1
            End While

        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try
        Return Index
    End Function

    Public Sub construct_Tree(ByVal fileName As String, ByVal Level As Integer)
        Try
            If Not (System.IO.File.Exists(fileName)) Then
                MsgBox(fileName & " does not exist.", MsgBoxStyle.OkOnly, "Warning")
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try

        If Level = 0 Then
            Dim Tree = New Node
            Index = CreateRootNode(Tree, fileName)
        Else
            Index = CreateNodesforLevels(Index, fileName, Level)
        End If
    End Sub

    Private Function CreateRootNode(ByVal Index As Node, ByVal fileName As String)
        Dim pages As New ArrayList
        Dim ranks As New ArrayList
        Dim keyValue As String
        Dim countPages As Integer
        Dim ListofValues() As String
        Dim countNodes As Integer = 0

        Dim readTreeStream As FileStream
        readTreeStream = System.IO.File.OpenRead(fileName)
        Dim readTreeStructure As New StreamReader(readTreeStream)
        Dim readLines As String
        Try
            While 1
                readLines = readTreeStructure.ReadLine()

                If readLines Is Nothing Then
                    Exit While
                End If

                If StrComp(readLines, "Node has...start", CompareMethod.Text) = 0 Then
                    countNodes = 0
                    Continue While
                End If
                If StrComp(readLines, "Node has...end", CompareMethod.Text) = 0 Then
                    Index.countNodes = countNodes
                    Continue While
                End If
                countPages = 0
                pages.Clear()
                ranks.Clear()

                ListofValues = readLines.Split(",")
                countPages = ListofValues.Length - 2
                keyValue = Trim(ListofValues(0))

                For count As Integer = 1 To countPages
                    pages.Add(Trim(ListofValues(count)))
                    'pages.Add(Trim(ListofValues(count).Split("-")(0)))
                    'ranks.Add(Trim(ListofValues(count).Split("-")(1)))
                Next

                Index.Value(countNodes) = keyValue
                Index.Pages(countNodes) = pages.ToArray("System.String".GetType)
                'Index.PageRank(countNodes) = ranks.ToArray("System.String".GetType)
                Index.CountPages(countNodes) = countPages

                countNodes = countNodes + 1
            End While

        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try

        Return Index

    End Function

    Private Function CreateNodesforLevels(ByVal root As Node, ByVal fileName As String, ByVal Level As Integer)
        Dim nextNode As Node

        Dim assignNode As Node
        nextNode = root

        Dim CountNodes As Integer = 0

        Try
            While 1
                If nextNode.ChildNodes(0) Is Nothing Then
                    Exit While
                Else
                    nextNode = nextNode.ChildNodes(0)
                End If
            End While

            Dim readTreeStream As FileStream
            readTreeStream = System.IO.File.OpenRead(fileName)
            Dim readTreeStructure As New StreamReader(readTreeStream)
            Dim readLines As String
            While 1
                readLines = readTreeStructure.ReadLine()
                If readLines Is Nothing Then
                    Exit While
                End If
                If StrComp(readLines, "Node has...start", CompareMethod.Text) = 0 Then
                    Dim newNode As New Node
                    assignNode = CreateNode(readTreeStructure, newNode)
                    assignNode.ParentNode = nextNode
                    nextNode.ChildNodes(CountNodes) = assignNode
                    If CountNodes = 0 Then
                        If Not nextNode.LeftNode Is Nothing Then
                            Dim prevNode As Node
                            If Not nextNode.LeftNode.ChildNodes(nextNode.LeftNode.countNodes) Is Nothing Then
                                prevNode = nextNode.LeftNode.ChildNodes(nextNode.LeftNode.countNodes)
                                assignNode.LeftNode = prevNode
                                prevNode.RightNode = assignNode
                            End If
                        End If
                    End If
                    If CountNodes > 0 AndAlso Not nextNode.ChildNodes(CountNodes - 1) Is Nothing Then
                        assignNode.LeftNode = nextNode.ChildNodes(CountNodes - 1)
                        assignNode.LeftNode.RightNode = assignNode
                    End If
                    'If Not nextNode.ChildNodes(CountNodes + 1) Is Nothing Then
                    '    assignNode.RightNode = nextNode.ChildNodes(CountNodes + 1)
                    'End If
                    CountNodes = CountNodes + 1
                    If CountNodes > nextNode.countNodes Then
                        CountNodes = 0
                        If Not nextNode.RightNode Is Nothing Then
                            nextNode = nextNode.RightNode
                        End If
                    End If
                End If
            End While

        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try

        Return root

    End Function

    Private Function CreateNode(ByRef readTreeStructure As StreamReader, ByVal Index As Node)
        Dim pages As New ArrayList
        Dim ranks As New ArrayList
        Dim keyValue As String
        Dim countPages As Integer
        Dim ListofValues() As String
        Dim countNodes As Integer = 0

        Dim readLines As String
        Try
            While 1
                readLines = readTreeStructure.ReadLine()
                If readLines Is Nothing Then
                    Exit While
                End If

                If StrComp(readLines, "Node has...end", CompareMethod.Text) = 0 Then
                    Index.countNodes = countNodes
                    Exit While
                End If

                countPages = 0
                pages.Clear()
                ranks.Clear()

                ListofValues = readLines.Split(",")
                countPages = ListofValues.Length - 2
                keyValue = Trim(ListofValues(0))

                For count As Integer = 1 To countPages
                    pages.Add(Trim(ListofValues(count)))
                    'pages.Add(Trim(ListofValues(count).Split("-")(0)))
                    'ranks.Add(Trim(ListofValues(count).Split("-")(1)))
                Next

                Index.Value(countNodes) = keyValue
                Index.Pages(countNodes) = pages.ToArray("System.String".GetType)
                'Index.PageRank(countNodes) = ranks.ToArray("System.String".GetType)
                Index.CountPages(countNodes) = countPages

                countNodes = countNodes + 1
            End While

        Catch ex As Exception
            MsgBox(ex.ToString(), MsgBoxStyle.Critical, "Error")
        End Try

        Return Index
    End Function

End Class
