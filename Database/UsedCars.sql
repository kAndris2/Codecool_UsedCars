--TABLES....................................................................................................
CREATE TABLE public.users
(
	id serial not null,
	name character varying(30) not null,
	registration_date bigint not null,
	gender bool,
	birth_date bigint,
	email character varying(30) not null,
	password character varying(20) not null,
	wallet int default 0,
	rank character varying(15),
	views int default 0,
	introduction character varying(500)
);

CREATE TABLE public.vehicles
(
	id serial not null,
	registration_date bigint not null,
	brand character varying(20) not null,
	model character varying(20) not null,
	vintage int not null,
	type character varying(20) not null,
	type_designation character varying(20) not null,
	price int not null,
	fuel character varying(20) not null,
	cylinder_capacity int not null,
	performance int not null,
	odometer int not null,
	description text not null,
	shop_id int,
	user_id int,
	views int default 0,
	votes int default 0,
	validity bool not null
);

CREATE TABLE public.shops
(
	id serial not null,
	name character varying(30) not null,
	registration_date bigint not null,
	description character varying(500),
	owner_id int not null,
	address character varying(50) not null,
	webpage character varying(30),
	views int default 0
);

CREATE TABLE public.comments
(
	id serial not null,
	title character varying(50) not null,
	message character varying(500) not null,
	submission_time bigint not null,
	owner_id int not null,
	user_id int,
	vehicle_id int,
	shop_id int
);

CREATE TABLE public.purchases
(
	id serial not null,
	shop_id int not null,
	amount int not null,
	year int not null,
	brand character varying(20) not null
);

CREATE TABLE public.pictures
(
	id serial not null,
	route character varying(300) not null,
	user_id int,
	vehicle_id int,
	shop_id int
);

CREATE TABLE public.races
(
	id serial not null,
	type character varying(20) not null
);

CREATE TABLE public.likes
(
	id serial not null,
	owner_id int not null,
	submission_time bigint not null,
	user_id int,
	shop_id int,
	vehicle_id int,
	comment_id int
);

--ADD PRIMARY KEY...........................................................................................
ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);
	
ALTER TABLE ONLY public.vehicles
    ADD CONSTRAINT vehicles_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.shops
    ADD CONSTRAINT shops_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.comments
    ADD CONSTRAINT comments_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.purchases
    ADD CONSTRAINT purchases_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.pictures
    ADD CONSTRAINT pictures_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.races
    ADD CONSTRAINT races_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.likes
    ADD CONSTRAINT likes_pkey PRIMARY KEY (id);

--ADD FOREIGN KEY...........................................................................................
ALTER TABLE ONLY public.vehicles
    ADD CONSTRAINT user_id FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.vehicles
    ADD CONSTRAINT shop_id FOREIGN KEY (shop_id) REFERENCES public.shops(id) ON DELETE CASCADE;
	

ALTER TABLE ONLY public.shops
    ADD CONSTRAINT owner_id FOREIGN KEY (owner_id) REFERENCES public.users(id) ON DELETE CASCADE;
	

ALTER TABLE ONLY public.comments
    ADD CONSTRAINT user_id FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.comments
    ADD CONSTRAINT vehicle_id FOREIGN KEY (vehicle_id) REFERENCES public.vehicles(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.comments
    ADD CONSTRAINT shop_id FOREIGN KEY (shop_id) REFERENCES public.shops(id) ON DELETE CASCADE;


ALTER TABLE ONLY public.purchases
    ADD CONSTRAINT shop_id FOREIGN KEY (shop_id) REFERENCES public.shops(id) ON DELETE CASCADE;


ALTER TABLE ONLY public.pictures
    ADD CONSTRAINT user_id FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.pictures
    ADD CONSTRAINT vehicle_id FOREIGN KEY (vehicle_id) REFERENCES public.vehicles(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.pictures
    ADD CONSTRAINT shop_id FOREIGN KEY (shop_id) REFERENCES public.shops(id) ON DELETE CASCADE;

ALTER TABLE ONLY public.likes
    ADD CONSTRAINT user_id FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.likes
    ADD CONSTRAINT vehicle_id FOREIGN KEY (vehicle_id) REFERENCES public.vehicles(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.likes
    ADD CONSTRAINT shop_id FOREIGN KEY (shop_id) REFERENCES public.shops(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.likes
    ADD CONSTRAINT comment_id FOREIGN KEY (comment_id) REFERENCES public.comments(id) ON DELETE CASCADE;