use domotic;

drop table temperature;
create table temperature (
	time DATETIME default getdate() primary key,
	value double precision not null
);

drop table light;
create table light (
	time datetime default getdate() primary key,
	turned_on bit not null
);

drop table presence;
create table presence (
	time datetime default getdate() primary key,
	presence bit not null
);

drop table luminosity;
CREATE TABLE luminosity(
	[time] [datetime] default getdate() primary key,
	[value] [float] NOT NULL
);

drop table heather;
create table heather (
	time datetime default getdate() primary key,
    turned_on bit not null
);