import React, { useState } from 'react';
import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import Select from '@mui/material/Select';
import { BarChart } from '@mui/x-charts/BarChart';
import { PieChart } from '@mui/x-charts/PieChart';
import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import MenuItem from '@mui/material/MenuItem';
import { axisClasses } from '@mui/x-charts/ChartsAxis';
import { mapNumberToMonthValue, getSum } from '../../helper';

const valueFormatter = (value) => `${value}₴`;

const chartSettingIncome = {
    yAxis: [
      {
        label: 'Money, ₴',
      },
    ],
    series: [{ dataKey: 'value', label: 'Money earned', valueFormatter }],
    height: 300,
    sx: {
      [`& .${axisClasses.directionY} .${axisClasses.label}`]: {
        transform: 'translateX(-10px)',
      },
    },
  };

  const chartSettingPurchase = {
      yAxis: [
        {
          label: 'Money, ₴',
        },
      ],
      series: [{ dataKey: 'value', label: 'Money spent', valueFormatter }],
      height: 300,
      sx: {
        [`& .${axisClasses.directionY} .${axisClasses.label}`]: {
          transform: 'translateX(-10px)',
        },
      },
    };

export default function MonthStatistic ({purchases, incomes}) {
    const [statisticType, setStatisticType] = useState(1);
    const [selectedMonth, setSelectedMonth] = useState(new Date().getMonth() + 1);

    const handleChange = (event, newAlignment) => {
        setStatisticType(newAlignment);
      };


    const handleMonthChange = (event) => {
        setSelectedMonth(event.target.value);
    };

      const getStats = (list, arrayToReturn) => {
        const filteredList = list.filter(x => x.month === selectedMonth);
        const grouppedItems = Object.groupBy(filteredList, x => x.categoryId);
          for (const month in grouppedItems) {
              arrayToReturn.push(
                  {
                      label: grouppedItems[month][0].category.name,
                      value: getSum(grouppedItems[month], 'price')
                  }
              );
          }
      }

    const getMonthlyStatistic = () => {
        const arrayToReturn = [];
        if(statisticType === 3)
        {
            getStats(incomes, arrayToReturn);
        } else if(statisticType === 1) {
            const mergedList = [...incomes, ...purchases];
            getStats(mergedList, arrayToReturn);
        } else {
            getStats(purchases, arrayToReturn);
        }

        return arrayToReturn;
    }

    const getBestDifference = (purchases, incomes) => {
      let difference = {category: '', value: 0};
      const grouppedPurchases = Object.groupBy(purchases, x => x.categoryId);
      const grouppedIncomes = Object.groupBy(incomes, x => x.categoryId);
      for (const categoryId in grouppedIncomes) {
        if(grouppedPurchases[categoryId] ) {
          const incomeSum = getSum(grouppedIncomes[categoryId], 'price');
          const purchaseSum = getSum(grouppedPurchases[categoryId], 'price');
          if(incomeSum - purchaseSum > difference.value){
            difference.value = incomeSum - purchaseSum;
            difference.category = grouppedPurchases[categoryId][0].category.name;;
          }
        } else {
          const incomeSum = getSum(grouppedIncomes[categoryId], 'price');
          if(incomeSum > difference.value){
            difference.value = incomeSum;
            difference.category = grouppedIncomes[categoryId][0].category.name;
          }
        }
      }

      return difference;
    }

    const getWorstDifference = (purchases, incomes) => {
      let difference = {category: '', value: 0};
      const grouppedPurchases = Object.groupBy(purchases, x => x.categoryId);
      const grouppedIncomes = Object.groupBy(incomes, x => x.categoryId);
      for (const categoryId in grouppedPurchases) {
        if(grouppedIncomes[categoryId] ) {
          const incomeSum = getSum(grouppedIncomes[categoryId], 'price');
          const purchaseSum = getSum(grouppedPurchases[categoryId], 'price');
          if(incomeSum - purchaseSum < difference.value){
            difference.value = incomeSum - purchaseSum;
            difference.category = grouppedPurchases[categoryId][0].category.name;
          }
        } else {
          const purchaseSum = getSum(grouppedPurchases[categoryId], 'price');
          if(-purchaseSum < difference.value){
            difference.value = -purchaseSum;
            difference.category = grouppedPurchases[categoryId][0].category.name;
          }
        }
      }

      return difference;
    }

    const getMonthListItem = (theBest) => {
      let difference = {}; 
      if(theBest)
        difference = getBestDifference(purchases.filter(x => x.month === selectedMonth), incomes.filter(x => x.month === selectedMonth));
      else
        difference = getWorstDifference(purchases.filter(x => x.month === selectedMonth), incomes.filter(x => x.month === selectedMonth));

      return `The ${(theBest ? 'best' : 'worst')} difference between expenses and incomes is in ${difference.category}, the difference is ${difference.value}`;
  }


    // use BarChart for categories
    return (
        <Box sx={{ mt: 1 }}>
        <Grid item xs={12}>
            <ToggleButtonGroup
                color="primary"
                value={statisticType}
                exclusive
                onChange={handleChange}
                aria-label="Platform"
                >
                <ToggleButton value={1}>Both</ToggleButton>
                <ToggleButton value={2}>Purchases</ToggleButton>
                <ToggleButton value={3}>Incomes</ToggleButton>
            </ToggleButtonGroup>
                <InputLabel id="month-select-label">Month</InputLabel>
                <Select
                labelId="month-select-label"
                id="month"
                name="month"
                label="Month"
                value={selectedMonth}
                // defaultValue={initialPurchase?.month ? initialPurchase?.month : ""}
                onChange={handleMonthChange}
                >
                <MenuItem value={1}>January</MenuItem>
                <MenuItem value={2}>February</MenuItem>
                <MenuItem value={3}>March</MenuItem>
                <MenuItem value={4}>April</MenuItem>
                <MenuItem value={5}>May</MenuItem>
                <MenuItem value={6}>June</MenuItem>
                <MenuItem value={7}>July</MenuItem>
                <MenuItem value={8}>August</MenuItem>
                <MenuItem value={9}>September</MenuItem>
                <MenuItem value={10}>October</MenuItem>
                <MenuItem value={11}>November</MenuItem>
                <MenuItem value={12}>December</MenuItem>
                </Select>
                </Grid>
        <PieChart series={[
            {
            data: getMonthlyStatistic(),
            innerRadius: 150,
            },
        ]}
        width={800}
        height={400}
        />
        <List dense={false}>
                <ListItem>
                  <ListItemText
                    primary={getMonthListItem(true)}
                  />
                </ListItem>
                <ListItem>
                  <ListItemText
                    primary={getMonthListItem(false)}
                  />
                </ListItem>
            </List>
        </Box>
    );
}
