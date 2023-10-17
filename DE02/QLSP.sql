USE MASTER
GO
IF EXISTS ( SELECT * FROM SYS.DATABASES WHERE NAME = 'QLSanpham')
    DROP DATABASE QLSanpham
GO

CREATE DATABASE QLSanpham
GO

USE QLSanpham
GO
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE NAME = 'LoaiSP')
    DROP TABLE LoaiSP
GO
CREATE TABLE LoaiSP
(
    MaLoai char(2) not null primary key,
    TenLoai nvarchar(30) not null
)

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE NAME = 'SanPham')
    DROP TABLE SanPham
GO
CREATE TABLE SanPham
(
    MaSP varchar(6) not null primary key,
    TenSP nvarchar(30),
    Ngaynhap datetime,
    MaLoai char(2)
)

ALTER TABLE SanPham
    ADD CONSTRAINT FK_SanPham_LoaiSP FOREIGN KEY(MALoai) REFERENCES LoaiSP(MaLoai)

SET DATEFORMAT DMY

INSERT INTO LoaiSP(MaLoai,TenLoai) VALUES('L1','Thuc an')
INSERT INTO LoaiSP(MaLoai,TenLoai) VALUES('L2','Nuoc uong')

INSERT INTO SanPham(MaSP,TenSP,Ngaynhap,MaLoai) VALUES('SP1','Banh Lays','6/11/2003','L1')
INSERT INTO SanPham(MaSP,TenSP,Ngaynhap,MaLoai) VALUES('SP2','CoCaCoLa','18/6/2003','L2')
INSERT INTO SanPham(MaSP,TenSP,Ngaynhap,MaLoai) VALUES('SP3','Pepsi','15/4/2003','L2')
INSERT INTO SanPham(MaSP,TenSP,Ngaynhap,MaLoai) VALUES('SP4','Snack oshi','11/8/2003','L1')

select * from LoaiSP
select * from SanPham