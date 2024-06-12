import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import InputLabel from '@mui/material/InputLabel';
import Select from '@mui/material/Select';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Box from '@mui/material/Box';


export default function ExcelReport () {
    const [error, setError] = useState(false);

  const populateExcel = (event) => {
    event.preventDefault();
    const data = new FormData(event.target);
    const year = data.get('year');
    var re = new RegExp("(19[89][0-9]|20[0-4][0-9]|2050)");
    if(re.test(year))
      {
        setError(false);
        // const downloadExcelResponse = fetch(`api/Reporting/getReport?year=${year}`);
        const blabla = fetch(`api/Reporting/excelGenSaved?year=${year}`);
      }
      else{
        setError(true);
      }
  }

    return (
      <div>
        <Box component="form" onSubmit={populateExcel} noValidate sx={{ mt: 1 }}>
        <Grid item xs={12}>
            <Typography sx={{ mt: 4, mb: 2 }} variant="h6" component="div">
            Excel is widely used for creating reports due to several key features and benefits:
            </Typography>
            <ul>
              <li>Versatility: Excel can handle various types of data, including numbers, text, dates, and formulas, making it suitable for diverse reporting needs.</li>
              <li>Formulas and Functions: Excel includes a vast library of built-in formulas and functions that enable complex calculations and data manipulation, enhancing the depth and accuracy of reports.</li>
              <li>Data Analysis Tools: It offers powerful tools for data analysis, such as pivot tables, charts, and graphs, which help in summarizing and visualizing data effectively.</li>
              <li>Data Import/Export: Excel can import data from and export data to various formats and sources, such as CSV files, databases, and other software, facilitating seamless integration and data sharing.</li>
            </ul>
            <Typography sx={{ mt: 4, mb: 2 }} variant="h6" component="div">
                This page allows you to download an excel file with year statistics.
            </Typography>
          
          <TextField
            sx={{ mb: 4, ml: 6, mr: 6 }}
            required
            error={error}
            id="year"
            name="year"
            label="Year"
            defaultValue={new Date().getFullYear()}
            variant="filled"
          />
          <Button type="submit" variant="outlined" sx={{ mb: 4 }} >
            Generate  Excel
          </Button>
          </Grid>
        </Box>
      </div>
    );
}
