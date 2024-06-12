import React, { useState } from 'react';
import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import { BarChart } from '@mui/x-charts/BarChart';
import Box from '@mui/material/Box';
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

const YearStatistic = ({purchases, incomes}) => {
    const [statisticType, setStatisticType] = useState(1);

    const handleChange = (event, newAlignment) => {
        setStatisticType(newAlignment);
      };

      const getStats = (list, arrayToReturn) => {
          const grouppedItems = Object.groupBy(list, x => x.month);
          for (const month in grouppedItems) {
              arrayToReturn.push(
                  {
                      month: mapNumberToMonthValue[month].substr(0, 3),
                      value: getSum(grouppedItems[month], 'price')
                  }
              );
          }
      }

    const getBestDifference = (purchases, incomes) => {
      let difference = {month: '', value: 0};
      const grouppedPurchases = Object.groupBy(purchases, x => x.month);
      const grouppedIncomes = Object.groupBy(incomes, x => x.month);
      for (const month in grouppedIncomes) {
        if(grouppedPurchases[month]) {
          const incomeSum =  getSum(grouppedIncomes[month], 'price')
          const purchaseSum = getSum(grouppedPurchases[month], 'price');
          if(incomeSum - purchaseSum > difference.value){
            difference.value = incomeSum - purchaseSum;
            difference.month = mapNumberToMonthValue[month];
          }
        } else {
          const incomeSum = getSum(grouppedIncomes[month], 'price');
          if(incomeSum > difference.value){
            difference.value = incomeSum;
            difference.category = mapNumberToMonthValue[month];
          }
        }
      }

      return difference;
    }

    const getWorstDifference = (purchases, incomes) => {
      let difference = {month: '', value: Infinity};
      const grouppedPurchases = Object.groupBy(purchases, x => x.month);
      const grouppedIncomes = Object.groupBy(incomes, x => x.month);
      for (const month in grouppedPurchases) {
        if(grouppedIncomes[month]) {
          const incomeSum =  getSum(grouppedIncomes[month], 'price')
          const purchaseSum = getSum(grouppedPurchases[month], 'price');
          if(incomeSum - purchaseSum < difference.value){
            difference.value = incomeSum - purchaseSum;
            difference.month = mapNumberToMonthValue[month];
          }
        } else {
          const purchaseSum = getSum(grouppedPurchases[month], 'price');
          if(-purchaseSum < difference.value){
            difference.value = -purchaseSum;
            difference.category = mapNumberToMonthValue[month];
          }
        }
      }

      return difference;
    }

    const getYearlyStatistic = () => {
        const arrayToReturn = [];
        if(statisticType === 2)
        {
            getStats(incomes, arrayToReturn);
        } else {
            getStats(purchases, arrayToReturn);
        }

        return arrayToReturn;
    }

    const getMonthListItem = (theBest) => {
      let difference = {}; 
      if(theBest)
        difference = getBestDifference(purchases, incomes);
      else
        difference = getWorstDifference(purchases, incomes);

      return `The ${(theBest ? 'best' : 'worst')} difference between expenses and incomes is in ${difference.month}, the difference is ${difference.value}`;
  }

    // use BarChart for categories
    return (
        <Box sx={{ mt: 1 }}>
            <ToggleButtonGroup
                color="primary"
                value={statisticType}
                exclusive
                onChange={handleChange}
                aria-label="Platform"
                >
                <ToggleButton value={1}>Purchases</ToggleButton>
                <ToggleButton value={2}>Incomes</ToggleButton>
            </ToggleButtonGroup>
            <BarChart
                dataset={getYearlyStatistic()}
                xAxis={[
                { scaleType: 'band', dataKey: 'month', tickPlacement: 'middle', tickLabelPlacement: 'middle' },
                ]}
                {...(statisticType === 2 ? chartSettingIncome : chartSettingPurchase)}
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

export default YearStatistic;