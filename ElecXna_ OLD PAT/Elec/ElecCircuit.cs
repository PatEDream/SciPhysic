using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QM.Util;

namespace QM.Elec
{
    public class ElecCircuit
    {
        //éléments rentrés
        protected List<ElecDipole> m_LDipole = new List<ElecDipole>();

        //éléments calculés
        protected Boolean m_IsDurty = true;
        protected int m_DurtyCount = 0;
        protected List<ElecNode> m_LNode = new List<ElecNode>();
        protected List<EWire> m_LWire = new List<EWire>();
        protected List<EWireConnection> m_LWireConnection = new List<EWireConnection>();

        protected float m_PotentialToIntensityConvertor = 10.0f;
        static public float m_IntensityOfShortCut = 10.0f;

        public void AddDipole(ElecDipole _ed)
        {
            m_LDipole.Add(_ed);
            m_IsDurty = true;
        }
        public void RemoveDipole(ElecDipole _ed)
        {
            m_LDipole.Remove(_ed);
            m_IsDurty = true;
        }


        void AddNodes(ElecDipole ed)
        {
            Boolean StartFound = false;
            foreach (ElecNode en in m_LNode)
            {
                if (en.m_Pos == ed.m_Start.m_Pos)
                {
                    en.m_LDipole.Add(ed);
                    ed.m_Start = en;
                    StartFound = true;
                    break;
                }
            }
            if (!StartFound)
            {
                ed.m_Start.m_LDipole.Add(ed);
                m_LNode.Add(ed.m_Start);
            }
            Boolean EndFound = false;
            foreach (ElecNode en in m_LNode)
            {
                if (en.m_Pos == ed.m_End.m_Pos)
                {
                    en.m_LDipole.Add(ed);
                    ed.m_End = en;
                    EndFound = true;
                    break;
                }
            }
            if (!EndFound)
            {
                ed.m_End.m_LDipole.Add(ed);
                m_LNode.Add(ed.m_End);
            }
        }

        protected Boolean ContinueTryAddSegment(EWire ew, ElecDipole es)
        {
            if (es.m_DurtyCount != m_DurtyCount && es.m_IsWire)
            {
                ew.m_LWireSegment.Add(es);
                return true;
                //es.m_DurtyCount = m_DurtyCount;
            }
            return false;
        }
        protected void ContinueWire(EWire ew)
        {
            Boolean DoContinue = false;
            Boolean DoBreak = false;
            foreach (EWireSegment es in ew.m_LWireSegment)
            {
                if (es.m_DurtyCount != m_DurtyCount)
                {
                    es.m_DurtyCount = m_DurtyCount;
                    if (es.m_Start.m_LDipole.Count() == 2)
                    {
                        if (ContinueTryAddSegment(ew, es.m_Start.m_LDipole[0])
                        || ContinueTryAddSegment(ew, es.m_Start.m_LDipole[1]))
                        {
                            es.m_Start.m_DurtyCount = m_DurtyCount;
                            DoBreak = true;
                        }
                    }
                    if (es.m_End.m_LDipole.Count() == 2)
                    {
                        if (ContinueTryAddSegment(ew, es.m_End.m_LDipole[0])
                        || ContinueTryAddSegment(ew, es.m_End.m_LDipole[1]))
                        {
                            es.m_End.m_DurtyCount = m_DurtyCount;
                            DoBreak = true;
                        }
                    }
                    DoContinue = true;
                    if (DoBreak)
                        break;
                }
            }

            if (DoContinue)
                ContinueWire(ew);
        }
        protected void ContinueWireConnection(EWireConnection ewc)
        {
            Boolean DoContinue = false;
            Boolean DoBreak = false;
            foreach (ElecNode en in ewc.m_LNode)
            {
                if (en.m_DurtyCount != m_DurtyCount)
                {
                    en.m_DurtyCount = m_DurtyCount;
                    foreach (ElecDipole ed in en.m_LDipole)
                    {
                        if (ed.m_IsWire)
                        {
                            ElecNode e0 = ed.m_Start;
                            if (e0.m_DurtyCount != m_DurtyCount)
                            {
                                ewc.m_LNode.Add(e0);
                                DoBreak = true;
                            }
                            ElecNode e1 = ed.m_End;
                            if (e1.m_DurtyCount != m_DurtyCount)
                            {
                                ewc.m_LNode.Add(e1);
                                DoBreak = true;
                            }
                        }
                    }

                    DoContinue = true;
                    if (DoBreak == true)
                        break;
                }
            }

            if (DoContinue)
                ContinueWireConnection(ewc);
        }
        public void ComputeCircuit()
        {
            m_LNode.Clear();
            m_LWire.Clear();
            m_LWireConnection.Clear();

            foreach (ElecDipole ed in m_LDipole)
            {
                ed.Init();
            }

            foreach (ElecDipole ed in m_LDipole)
            {
                AddNodes(ed);
            }

            m_DurtyCount++;

            foreach (ElecDipole ed in m_LDipole)
            {
                if (ed.m_IsWire && ed.m_DurtyCount != m_DurtyCount)
                {
                    EWire ew = new EWire();
                    ew.m_LWireSegment.Add((EWireSegment)ed);
                    ContinueWire(ew);
                    ew.ChooseColorDebug();
                    m_LWire.Add(ew);
                }
            }

            m_DurtyCount++;

            foreach (ElecNode en in m_LNode)
            {
                if (en.m_DurtyCount != m_DurtyCount)
                {
                    //en.m_DurtyCount = m_DurtyCount;
                    EWireConnection ewc = new EWireConnection();
                    ewc.m_LNode.Add(en);
                    en.m_EWireConnection = ewc;
                    ContinueWireConnection(ewc);
                    ewc.ComputeCenter();
                    m_LWireConnection.Add(ewc);
                }

            }

            foreach (EWireConnection ewc in m_LWireConnection)
            {
                foreach (ElecNode en in ewc.m_LNode)
                {
                    en.m_EWireConnection = ewc;
                }
            }

            ///////////////////
            //Tests de debug ....
            //////////////////
            foreach (ElecDipole ed in m_LDipole)
            {
                Boolean StartFound = false;
                Boolean EndFound = false;
                foreach (ElecNode en in m_LNode)
                {
                    if (en == ed.m_Start)
                        StartFound = true;
                    if (en == ed.m_End)
                        EndFound = true;
                }
                if (!StartFound || !EndFound)
                    DebugManager.DoArret();
            }
            foreach (ElecNode en in m_LNode)
            {
                if (en.m_EWireConnection == null)
                    DebugManager.DoArret();
            }
            m_IsDurty = false;


        }
        public void SetDurty()
        {
            m_IsDurty = true;
           // ComputeCircuit();
        }
        public void EmptyActivity()
        {
            foreach (ElecDipole ed in m_LDipole)
            {
                ed.m_Potentiel = 0.0f;
                ed.m_Intensity = 0.0f;
            }
            foreach (ElecNode en in m_LNode)
            {
                en.m_Potentiel = 0.0f;
            }
            foreach (EWire ew in m_LWire)
            {
                ew.m_Potentiel = 0.0f;
                ew.m_Intensity = 0.0f;
            }
            foreach (EWireConnection ewc in m_LWireConnection)
            {
                ewc.m_Potentiel = 0.0f;
            }
        }

        public Boolean ComputeLoiDesNoeuds(ElecNode en, ElecDipole edMe)
        {
            if (en.m_LDipole.Count() == 1)
            {
                edMe.m_Intensity = 0.0f;
                return true;
            }
            int Count = 0;
            float Intensity = 0.0f;
            foreach (ElecDipole edOther in en.m_LDipole)
            {
                if (edOther != edMe)
                {
                    if (edOther.m_IsWire == false
                        || (edOther.m_DurtyCount == m_DurtyCount))  //C'est un Wire mais déjà calculé
                    {
                        //Loi des noeuds
                        if (edOther.m_Start == edMe.m_Start || edOther.m_End == edMe.m_End)
                            Intensity -= edOther.m_Intensity;
                        else
                            Intensity += edOther.m_Intensity;
                        Count++;
                    }
                }
            }
            if (Count == en.m_LDipole.Count() - 1)
            {
                edMe.m_Intensity = Intensity;
                return true;
            }
            return false;
        }
        public Boolean CanForceLoiDesNoeuds(ElecNode en, ElecDipole edMe, Boolean AcceptNull)
        {
            if (en.m_LDipole.Count() <= 2)
            {
                return false;
            }
            int Count = 0;
            float Intensity = 0.0f;
            foreach (ElecDipole edOther in en.m_LDipole)
            {
                if (edOther != edMe)
                {
                    if (edOther.m_IsWire == false
                        || (edOther.m_DurtyCount == m_DurtyCount))  //C'est un Wire mais déjà calculé
                    {
                        //Loi des noeuds
                        if (edOther.m_Start == edMe.m_Start || edOther.m_End == edMe.m_End)
                            Intensity -= edOther.m_Intensity;
                        else
                            Intensity += edOther.m_Intensity;
                        Count++;
                    }
                }
            }
            if (Count > 0 && Count < en.m_LDipole.Count() - 1 && en.m_LDipole.Count()-Count<3)
            {
                if (AcceptNull || Math.Abs(Intensity) > 0.0001f) //Intensity!=0.0f) //
                {
                    int N = (en.m_LDipole.Count() - Count);
                    edMe.m_Intensity = Intensity / N;
                    return true;
                }
            }
            return false;
        }
        public void DoOneStep()
        {
            if (m_IsDurty)
                ComputeCircuit();

            //Calcul par itération du potentiel et de l'intensité sur le circuit
            for (int i = 0; i < 1000; i++)
            {
                foreach (ElecDipole ed in m_LDipole)
                {
                    ed.DoOneStep(m_PotentialToIntensityConvertor);
                }
            }

            //duplicage du potentiel calculé sur les WireConnection
            // sur chacun de leurs Wire
            foreach (EWireConnection ewc in m_LWireConnection)
            {
                foreach (ElecNode en in ewc.m_LNode)
                {
                    foreach (ElecDipole ed in en.m_LDipole)
                    {
                        if (ed.m_IsWire)
                            ed.m_Potentiel = ewc.m_Potentiel;
                    }
                }
            }

            //marquage de l'intensité dans les Wire
            // (il n'a été calculé que dans les autres dipoles)
            m_DurtyCount++;
            Boolean Found = true;
            Boolean FoundLoiDesNoeuds = true;
            int CountLoop = 0;
            while (Found && CountLoop<1000)
            {
                CountLoop++;
                Found = false;
                FoundLoiDesNoeuds = false;
                foreach (ElecDipole ed in m_LDipole)
                {
                    if (ed.m_IsWire)
                    {
                        if (ed.m_DurtyCount != m_DurtyCount)
                        {
                            Found = true;
                            if (ComputeLoiDesNoeuds(ed.m_Start, ed))
                            {
                                ed.m_DurtyCount = m_DurtyCount;
                                FoundLoiDesNoeuds = true;
                            }
                            else
                            {
                                if (ComputeLoiDesNoeuds(ed.m_End, ed))
                                {
                                    ed.m_DurtyCount = m_DurtyCount;
                                    FoundLoiDesNoeuds = true;
                                }
                            }
                        }
                    }
                }

                //on traite les boucles restantes
                // Au premier passage on ne traite que celle avec l'intensity non nulle
                // Au second passage on traite finalement celles d'intensity nulle
                for(int i=0; i<2; i++)
                {
                    if (!FoundLoiDesNoeuds && Found) 
                    {
                        foreach (ElecDipole ed in m_LDipole)
                        {
                            if (ed.m_IsWire)
                            {
                                if (ed.m_DurtyCount != m_DurtyCount)
                                {
                                    if (CanForceLoiDesNoeuds(ed.m_Start, ed, i==1))
                                    {
                                        ed.m_DurtyCount = m_DurtyCount;
                                        FoundLoiDesNoeuds = Found = true;
                                        break;
                                    }
                                    else
                                    {
                                        if (CanForceLoiDesNoeuds(ed.m_End, ed, i==1))
                                        {
                                            ed.m_DurtyCount = m_DurtyCount;
                                            FoundLoiDesNoeuds = Found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public void DrawDebug(Color[] _BlockTextureData, int sx, int sy)
        {
            foreach (EWireConnection ewc in m_LWireConnection)
            {
                ewc.DrawDebug(_BlockTextureData, sx, sy);
            }
            foreach (ElecDipole ed in m_LDipole)
                ed.DrawDebug(_BlockTextureData, sx, sy);
        }

        public void DrawText(SpriteBatch spriteBatch)
        {
            foreach (ElecDipole ed in m_LDipole)
                ed.DrawText(spriteBatch);
            foreach (EWireConnection ewc in m_LWireConnection)
                ewc.DrawText(spriteBatch);
        }

        public int NbLightON()
        {
            int nb = 0;
            foreach (ElecDipole ed in m_LDipole)
            {
                if (ed.m_IsLight && ed.IsON())
                    nb++;
            }
            return nb;
        }
    }
}
