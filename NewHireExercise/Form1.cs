/*Author: Lee Synkowski
 *Project:NewHireExercise
 *Description: Window GUI app that allows the user to pick a text
 *file of race participants, assign age group rankings, and display
 *the overall results of the race.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace NewHireExercise
{
    public partial class Form1 : Form
    {
        private List<RacerData> allRacers = new List<RacerData>();
        private List<RacerData> racers15AndUnder = new List<RacerData>();
        private List<RacerData> racers16To29Under = new List<RacerData>();
        private List<RacerData> racers30AndOver = new List<RacerData>();

        public Form1()
        {
            InitializeComponent();
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                string line;
                
                //read first two lines of file which contain irrelevant info
                sr.ReadLine();
                sr.ReadLine();

                while ((line = sr.ReadLine()) != null)
                {
                    //split on valid words
                    string[] lineData = Regex.Split(line, @"\W+");
                    
                    if (lineData.Length == 3)
                    {
                        int time;
                        int age;
                        RacerData currentRacer = null;
                        
                        //error check for bad values in the input
                        //ignores rows with bad values so program continues to execute
                        try
                        {
                            time = Convert.ToInt32(lineData[1]);
                            age = Convert.ToInt32(lineData[2]);
                            currentRacer = new RacerData(lineData[0], Convert.ToInt32(lineData[1]), Convert.ToInt32(lineData[2]));
                        }
                        catch (FormatException exception)
                        {
                            //code to log exception to appropriate service can go here
                        }
                        catch (OverflowException exception)
                        {
                            //code to log exception to appropriate service can go here
                        }
                        
                        //assign a valid racer to the correct age group collection
                        if (currentRacer != null)
                        {
                            if (currentRacer.age <= 15)
                            {
                                racers15AndUnder.Add(currentRacer);
                            }
                            else if (currentRacer.age <= 29)
                            {
                                racers16To29Under.Add(currentRacer);
                            }
                            else
                            {
                                racers30AndOver.Add(currentRacer);
                            }
                        }
                    }
                }
                sr.Close();

                //sort the racers in each group by time
                racers15AndUnder = racers15AndUnder.OrderBy(RacerData => RacerData.time).ToList<RacerData>();
                racers16To29Under = racers16To29Under.OrderBy(RacerData => RacerData.time).ToList<RacerData>();
                racers30AndOver = racers30AndOver.OrderBy(RacerData => RacerData.time).ToList<RacerData>();
                
                //update the rankings of each racer
                assignRankings(racers15AndUnder);
                assignRankings(racers16To29Under);
                assignRankings(racers30AndOver);

                //combine the lists together
                allRacers.AddRange(racers15AndUnder);
                allRacers.AddRange(racers16To29Under);
                allRacers.AddRange(racers30AndOver);

                //sort by overall time
                allRacers = allRacers.OrderBy(RacerData => RacerData.time).ToList<RacerData>();

                //create & display output string
                string outString = "-------RACE RESULTS-------" + Environment.NewLine;
                outString += "Name" + "\t" + "Time" + "\t" + "Age" + "\t" + "Ranking" + Environment.NewLine;
                outString += "====" + "\t" + "====" + "\t" + "===" + "\t" + "=======" + Environment.NewLine; 
                foreach (RacerData rd in allRacers)
                {
                    outString += rd.toString() + Environment.NewLine;
                }
                
                displayBox.Text = outString;           
            }
        }

        //Assign rankings to the racers. ASSUMES NO TIES.
        //Racers with equal times will be given sequential placings.
        private void assignRankings(List<RacerData> racerDataList)
        {
            int rank = 1;
            foreach (RacerData rd in racerDataList)
            {
                rd.ranking = rank;
                rank++;
            }
        } 

        
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        
    }
}
