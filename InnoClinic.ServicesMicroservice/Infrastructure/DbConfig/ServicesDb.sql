if not exists(select * from sys.databases where name = 'InnoClinic_ServicesDB') 
begin
	create database InnoClinic_ServicesDB;
end

use InnoClinic_ServicesDB;

create table [ServiceCategories](
	[Id] uniqueidentifier unique DEFAULT NEWID() not null,
	[Name] nvarchar(40) not null,
	[TimeSlotSize] datetime not null,
	primary key (Id)
);

create table [Specializations](
	[Id] uniqueidentifier unique DEFAULT NEWID() not null,
	[Name] nvarchar(40) not null,
	[Status] nvarchar(20) not null,
	primary key (Id)
);

create table [Services](
	[Id] uniqueidentifier unique DEFAULT NEWID() not null,
	[Name] nvarchar(40) not null,
	[Price] decimal not null,
	[CategoryId] uniqueidentifier not null,
	[SpecializationId] uniqueidentifier,
	[Status] nvarchar(20) not null,
	primary key (Id),
	foreign key (CategoryId) references ServiceCategories (Id) on delete cascade,
	foreign key (SpecializationId) references Specializations (Id) on delete cascade
);



--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=SERVICES=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=--

go
create procedure CreateService
	@id uniqueidentifier,
	@name nvarchar(40),
	@price decimal,
	@categoryId uniqueidentifier,
	@specializationId uniqueidentifier,
	@status nvarchar(20)
as
begin
	insert into Services(
		[Id],
		[Name], 
		[Price], 
		[CategoryId], 
		[SpecializationId], 
		[Status]) 
	values (@id, @name, @price, @categoryId, @specializationId, @status)
end
go



go
create procedure GetMinServiceById
	@id uniqueidentifier
as
begin
	select * from Services
	where Id = @id

	select sc.Id, sc.Name, sc.TimeSlotSize from ServiceCategories as sc
	join dbo.Services as s on s.CategoryId = sc.Id
	where s.Id = @id
end
go



go
create procedure GetServiceById
	@id uniqueidentifier
as
begin
	select * from Services
	where Id = @id

	select sc.Id, sc.Name, sc.TimeSlotSize from ServiceCategories as sc
	join dbo.Services as s on s.CategoryId = sc.Id
	where s.Id = @id

	select sp.Id, sp.Name, sp.Status from Specializations as sp
	join dbo.Services as s on s.SpecializationId = sp.Id
	where s.Id = @id
end
go



go
create procedure GetServiceByName
	@name nvarchar(40)
as
begin
	select * from Services
	where Name = @name

	select sc.Id, sc.Name, sc.TimeSlotSize from ServiceCategories as sc
	join dbo.Services as s on s.CategoryId = sc.Id
	where s.Name = @name

	select sp.Id, sp.Name, sp.Status from Specializations as sp
	join dbo.Services as s on s.SpecializationId = sp.Id
	where s.Name = @name
end
go



go
create procedure GetServices
	@categoryName nvarchar(40),
	@specializationName nvarchar(40)
as
begin
	select s.Id as sId, 
		s.Name as sName, 
		s.Price as sPrice, 
		s.CategoryId sCategoryId, 
		s.SpecializationId sSpecializationId, 
		s.Status sStatus, 
		sc.Id scId, 
		sc.Name as scName, 
		sc.TimeSlotSize as scTimeSlotSize, 
		sp.Id as spId, 
		sp.Name as spName, 
		sp.Status as spStatus 
	into #GetServices_CommonResult from Services as s
	join ServiceCategories as sc on sc.Id = s.CategoryId
	join Specializations as sp on sp.Id = s.SpecializationId
	where (@categoryName is not null and sc.Name = @categoryName or @categoryName is null) and 
		(@specializationName is not null and sp.Name = @specializationName or @specializationName is null)


	select sId as Id, sName as Name, sPrice as Price, sCategoryId as CategoryId, sSpecializationId as SpecializationId, sStatus as Status from #GetServices_CommonResult
	select distinct scId as Id, scName as Name, scTimeSlotSize as TimeSlotSize from #GetServices_CommonResult
	select distinct spId as Id, spName as Name, spStatus as Status from #GetServices_CommonResult
end
go



go
create procedure ServiceExists
	@id uniqueidentifier
as
begin
	select count(Id) from Services
	where Id = @id

end
go



go
create procedure UpdateService
	@id uniqueidentifier,
	@name nvarchar(40),
	@price decimal,
	@categoryId uniqueidentifier,
	@specializationId uniqueidentifier,
	@status nvarchar(20)
as
begin
	update Services
	set 
		[Name] = @name, 
		[Price] = @price, 
		[CategoryId] = @categoryId,
		[SpecializationId] = @specializationId,
		[Status] = @status
	where Id = @id
end
go



go
create procedure ChangeServiceStatus
	@id uniqueidentifier,
	@status nvarchar(20)
as
begin
	update Services
	set [Status] = @status
	where Id = @id
end
go



go
create procedure LinkWithSpecialization
	@id uniqueidentifier,
	@specializationId uniqueidentifier
as
begin
	update Services
	set [SpecializationId] = @specializationId
	where Id = @id
end
go



--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=SERVICE_CATEGORY=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=--



go
create procedure GetServiceCategoryById
	@id uniqueidentifier
as
begin
	select * from ServiceCategories
	where Id = @id
end
go



go
create procedure GetServiceCategories
as
begin
	select * from ServiceCategories
end
go



--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=SPECIALIZATIONS=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=--



go
create procedure CreateSpecialization
	@id uniqueidentifier,
	@name nvarchar(40),
	@status nvarchar(20)
as
begin
	insert into Specializations(
		[Id],
		[Name], 
		[Status]) 
	values (@id, @name, @status)
end
go



go
create procedure GetSpecializationById
	@id uniqueidentifier
as
begin
	select * from Specializations
	where Id = @id
end
go



go
create procedure GetSpecializationByName
	@name nvarchar(40)
as
begin
	select * from Specializations
	where Name = @name
end
go



go
create procedure SpecializationExists
	@id uniqueidentifier
as
begin
	select count(Id) from Specializations
	where Id = @id

end
go



go
create procedure GetSpecializations
as
begin
	select * from Specializations
end
go



go
create procedure UpdateSpecialization
	@id uniqueidentifier,
	@name nvarchar(40),
	@status nvarchar(20)
as
begin
	update Specializations
	set 
		[Name] = @name, 
		[Status] = @status
	where Id = @id
end
go



go
create procedure ChangeSpecializationStatus
	@id uniqueidentifier,
	@status nvarchar(20)
as
begin
	update Specializations
	set [Status] = @status
	where Id = @id
end
go