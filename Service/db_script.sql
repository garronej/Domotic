use domotic;

create table temperature (
	time DATETIME default getdate() primary key,
	value double precision not null
);

create table light (
	time datetime default getdate() primary key,
	turned_on bit not null
);

create table presence (
	time datetime default getdate() primary key,
	presence bit not null
);

CREATE TABLE luminosity(
	[time] [datetime] default getdate() primary key,
	[value] [float] NOT NULL
);
create table heather (
	time datetime default getdate() primary key,
    turned_on bit not null
);