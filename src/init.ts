import dotenv from "dotenv";
import { createConnection } from "mysql2";
import { readFileSync } from "fs";

export class Init {
    constructor() {
        dotenv.config();

	this.init();
    }

    init() {
	const connection = createConnection({
	    host: process.env.MYSQL_HOST,
	    port: 3306,
	    user: process.env.MYSQL_USER,
	    password: process.env.MYSQL_PASSWORD,
	    database: process.env.MYSQL_DATABASE,
        });

	connection.connect();

	// tables
	connection.query(readFileSync("./data/tables/users.sql").toString());

	// views

	connection.end();

	console.log("Initialised Database");
    }
}

new Init();
