import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import FormControl from '@mui/material/FormControl';
import Tab from '@mui/material/Tab';
import TabContext from '@mui/lab/TabContext';
import TabList from '@mui/lab/TabList';
import TabPanel from '@mui/lab/TabPanel';
import Box from '@mui/material/Box';
import YearStatistic from './YearStatistic'
import MonthStatistic from './MonthStatistic'
import ExcelReport from './ExcelReport';


export default function Reporting () {
  const [tab, setTab] = useState(1);
  const [purchases, setPurchases] = useState([]);
  const [incomes, setIncomes] = useState([]);

  useEffect(() => {
      fetch(`api/income/GetIncomes`).then(response => response.json())
      .then(data => setIncomes(data));
    },
  [setIncomes]);

  useEffect(() => {    
      fetch(`api/Purchase/GetPurchases`).then(response => response.json())
      .then(data => setPurchases(data));
    },
  [setPurchases]);

  const handleChange = (event, newValue) => {
    setTab(newValue);
  };


    return (
      <div>
        <Box>
          <TabContext value={Number(tab)}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
              <TabList onChange={handleChange} aria-label="Reporting tabs">
                <Tab label="Excel Report" value={1} />
                <Tab label="Year Statistic" value={2} />
                <Tab label="Month Statistic" value={3} />
                <Tab label="Predictions" value={4} disabled/>
              </TabList>
            </Box>
            <TabPanel value={1}><ExcelReport/></TabPanel>
            <TabPanel value={2}><YearStatistic purchases={purchases} incomes={incomes}/></TabPanel>
            <TabPanel value={3}><MonthStatistic purchases={purchases} incomes={incomes}/></TabPanel>
            <TabPanel value={4}></TabPanel>
          </TabContext>
        </Box>
      </div>
    );

}
