CREATE DATABASE DataCollectionSystem
GO
USE [DataCollectionSystem]
GO
CREATE TABLE Operators
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
	[AlgorithmModel] [nvarchar](50) NOT NULL,
	[OperatorModel] [nvarchar](50) NOT NULL,
	PRIMARY KEY (ID)
)
GO
CREATE TABLE Realizations
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[SignName] [nvarchar](1) NOT NULL,
	PRIMARY KEY (ID)
)
GO
CREATE TABLE Operations
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
	[AlgorithmModel] [nvarchar](100) NOT NULL,
	PRIMARY KEY (ID)
)
GO
INSERT INTO Operators (Name, Description, AlgorithmModel, OperatorModel)
		VALUES ('A(B)', '������ ����������', 'A(B)', 'A(B)');
INSERT INTO Operators (Name, Description, AlgorithmModel, OperatorModel)
		VALUES ('A(E)', '����� ����������', 'A(E)', 'A(E)');
INSERT INTO Operators (Name, Description, AlgorithmModel, OperatorModel)
		VALUES ('<<', '������ �����', '<<', 'F(op)[<<]');
INSERT INTO Operators (Name, Description, AlgorithmModel, OperatorModel)
		VALUES ('>>', '����� �����', '>>', 'F(op)[>>]');
INSERT INTO Operators (Name, Description, AlgorithmModel, OperatorModel)
		VALUES ('[', '����������� ������', '[', '[');
INSERT INTO Operators (Name, Description, AlgorithmModel, OperatorModel)
		VALUES (']', '����������� ������', ']', ']');
INSERT INTO Operators (Name, Description, AlgorithmModel, OperatorModel)
		VALUES ('||', '���', '||', '||');
GO
INSERT INTO Realizations (Name, SignName)
		VALUES ('����������', '�');
INSERT INTO Realizations (Name, SignName)
		VALUES ('�����������', 'p');
GO
INSERT INTO Operations (Name, Description, AlgorithmModel)
		VALUES ('C(p1/p2)', '������������� ���������', 'E(C;(p1);(p2))');
INSERT INTO Operations (Name, Description, AlgorithmModel)
		VALUES ('F(p1,p2)', '���������� �������', 'E(C:N(p1);(p1);(p2))');
INSERT INTO Operations (Name, Description, AlgorithmModel)
		VALUES ('I(p1/p2)', '���������', 'E(S:N,g;(p1);(p2,p3))');
INSERT INTO Operations (Name, Description, AlgorithmModel)
		VALUES ('delay(t)', '��������', 'E(C:I(p1*b(T-t));(p1);(p2))');
GO